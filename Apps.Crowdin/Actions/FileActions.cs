using System.Net.Mime;
using Apps.Crowdin.Api;
using Apps.Crowdin.Invocables;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.File;
using Apps.Crowdin.Models.Request.Project;
using Apps.Crowdin.Models.Response.File;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Parsers;
using Blackbird.Applications.Sdk.Utils.Utilities;
using Crowdin.Api;
using Crowdin.Api.SourceFiles;
using RestSharp;

namespace Apps.Crowdin.Actions;

[ActionList]
public class FileActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : AppInvocable(invocationContext)
{
    [Action("Search files", Description = "List project files")]
    public async Task<ListFilesResponse> ListFiles(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] ListFilesRequest input)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
        var intBranchId = IntParser.Parse(input.BranchId, nameof(input.BranchId));
        var intDirectoryId = IntParser.Parse(input.DirectoryId, nameof(input.DirectoryId));

        var items = await Paginator.Paginate((lim, offset) =>
        {
            var request = new FilesListParams
            {
                BranchId = intBranchId,
                DirectoryId = intDirectoryId,
                Filter = input.Filter,
                Limit = lim,
                Offset = offset
            };

            return ExceptionWrapper.ExecuteWithErrorHandling(() => 
                SdkClient.SourceFiles.ListFiles<FileCollectionResource>(intProjectId!.Value, request));
        });

        var files = items.Select(x => new FileEntity(x)).ToArray();
        return new(files);
    }

    [Action("Get file", Description = "Get specific file info")]
    public async Task<FileEntity> GetFile(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] FileRequest fileRequest)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
        var intFileId = IntParser.Parse(fileRequest.FileId, nameof(fileRequest.FileId));

        var file = await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
            await SdkClient.SourceFiles.GetFile<FileResource>(intProjectId!.Value, intFileId!.Value));
        return new(file);
    }

    [Action("Add file", Description = "Add new file")]
    public async Task<FileEntity> AddFile(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] AddNewFileRequest input)
    {
        if (input.StorageId is null && input.File is null)
        {
            throw new PluginMisconfigurationException("You need to specify one of the parameters: Storage ID or File");
        }

        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
        var intStorageId = LongParser.Parse(input.StorageId, nameof(input.StorageId));
        var intBranchId = IntParser.Parse(input.BranchId, nameof(input.BranchId));
        var intDirectoryId = IntParser.Parse(input.DirectoryId, nameof(input.DirectoryId));

        var fileName = input.Name ?? input.File?.Name;

        if (intStorageId is null && input.File != null)
        {
            var fileStream = await fileManagementClient.DownloadAsync(input.File);
            var storage = await SdkClient.Storage
                .AddStorage(fileStream, fileName!);
            intStorageId = storage.Id;
        }

        if (input.File is null)
        {
            var storage = await new StorageActions(InvocationContext, fileManagementClient).GetStorage(new()
            {
                StorageId = intStorageId.ToString()!
            });

            fileName = storage.FileName;
        }

        try
        {
            var request = new AddFileRequest
            {
                StorageId = intStorageId!.Value,
                Name = fileName!,
                BranchId = intBranchId,
                DirectoryId = intDirectoryId,
                Title = input.Title,
                ExcludedTargetLanguages = input.ExcludedTargetLanguages?.ToList(),
                AttachLabelIds = input.AttachLabelIds?.ToList(),
                Context = input.Context
            };

            var file = await SdkClient.SourceFiles.AddFile(intProjectId!.Value, request);
            return new(file);
        }
        catch (CrowdinApiException ex)
        {
            if (!ex.Message.Contains("Name must be unique"))
            {
                throw new PluginMisconfigurationException(ex.Message);
            }

            var allFiles = await ListFiles(project, new());
            var fileToUpdate = allFiles.Files.First(x => x.Name == fileName);

            return await UpdateFile(project, new()
            {
                FileId = fileToUpdate.Id
            }, new()
            {
                StorageId = intStorageId.ToString()
            }, new());
        }
    }

    [Action("Update file", Description = "Update an existing file with new content")]
    public async Task<FileEntity> UpdateFile(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] FileRequest file,
        [ActionParameter] ManageFileRequest input,
        [ActionParameter] UpdateFileRequest updateFileRequest)
    {
        if (input.StorageId is null && input.File is null)
        {
            throw new PluginMisconfigurationException("You need to specify one of the parameters: Storage ID or File");
        }

        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
        var intStorageId = LongParser.Parse(input.StorageId, nameof(input.StorageId));
        var intFileId = IntParser.Parse(file.FileId, nameof(file.FileId));

        var client = SdkClient;

        if (intStorageId is null)
        {
            var fileStream = await fileManagementClient.DownloadAsync(input.File);
            var storage = await ExceptionWrapper.ExecuteWithErrorHandling(async () => await client.Storage
                .AddStorage(fileStream, input.File.Name));

            intStorageId = storage.Id;
        }

        var request = new ReplaceFileRequest
        {
            StorageId = intStorageId.Value,
            UpdateOption = ToOptionEnum(updateFileRequest.UpdateOption)
        };

        var (result, isModified) = await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
            await client.SourceFiles.UpdateOrRestoreFile(
                intProjectId!.Value,
                intFileId!.Value,
                request));

        return new(result, isModified);
    }

    [Action("Download file", Description = "Download specific file")]
    public async Task<DownloadFileResponse> DownloadFile(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] FileRequest file)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
        var intFileId = IntParser.Parse(file.FileId, nameof(file.FileId));

        var client = SdkClient;

        var downloadLink = await client.SourceFiles.DownloadFile(intProjectId!.Value, intFileId!.Value);

        var fileInfo = await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
            await client.SourceFiles.GetFile<FileResource>(intProjectId!.Value, intFileId!.Value));
        var fileContent = await FileDownloader.DownloadFileBytes(downloadLink.Url);

        fileContent.Name = fileInfo.Name;
        fileContent.ContentType = fileContent.ContentType == MediaTypeNames.Text.Plain
            ? MediaTypeNames.Application.Octet
            : fileContent.ContentType;

        var fileReference =
            await fileManagementClient.UploadAsync(fileContent.FileStream, fileContent.ContentType, fileContent.Name);
        return new(fileReference);
    }

    [Action("Add or update file", Description = "Add or update file")]
    public async Task<FileEntity> AddOrUpdateFile(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] AddOrUpdateFileRequest input)
    {
        var projectFiles = await ListFiles(project, new ListFilesRequest());
        var existingFile = projectFiles.Files.FirstOrDefault(f => f.Name == input.File.Name);
        if (existingFile != null)
        {
            return await UpdateFile(project, new() { FileId = existingFile.Id }, new() { File = input.File },
                new() { UpdateOption = input.UpdateOption });
        }

        return await AddFile(project, input);
    }

    [Action("Delete file", Description = "Delete specific file")]
    public async Task DeleteFile(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] [Display("File ID")] string fileId)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
        var intFileId = IntParser.Parse(fileId, nameof(fileId));
        await ExceptionWrapper.ExecuteWithErrorHandling(async () => 
            await SdkClient.SourceFiles.DeleteFile(intProjectId!.Value, intFileId!.Value));
    }

    private FileUpdateOption? ToOptionEnum(string? option)
    {
        if (option == "keep_translations")
            return FileUpdateOption.KeepTranslations;

        if (option == "keep_translations_and_approvals")
            return FileUpdateOption.KeepTranslationsAndApprovals;

        if (option == "clear_translations_and_approvals")
            return FileUpdateOption.ClearTranslationsAndApprovals;

        return null;
    }
}