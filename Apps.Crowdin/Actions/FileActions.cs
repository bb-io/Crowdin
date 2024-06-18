using System.Reflection;
using Apps.Crowdin.Api;
using Apps.Crowdin.Models.Dtos;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.File;
using Apps.Crowdin.Models.Request.Project;
using Apps.Crowdin.Models.Response.File;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Parsers;
using Crowdin.Api;
using Crowdin.Api.SourceFiles;
using Crowdin.Api.Storage;
using RestSharp;

namespace Apps.Crowdin.Actions;

[ActionList]
public class FileActions : BaseInvocable
{
    private AuthenticationCredentialsProvider[] Creds =>
        InvocationContext.AuthenticationCredentialsProviders.ToArray();

    private readonly IFileManagementClient _fileManagementClient;

    public FileActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : base(
        invocationContext)
    {
        _fileManagementClient = fileManagementClient;
    }

    [Action("List files", Description = "List project files")]
    public async Task<ListFilesResponse> ListFiles(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] ListFilesRequest input)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
        var intBranchId = IntParser.Parse(input.BranchId, nameof(input.BranchId));
        var intDirectoryId = IntParser.Parse(input.DirectoryId, nameof(input.DirectoryId));

        var client = new CrowdinClient(Creds);

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

            return client.SourceFiles.ListFiles<FileCollectionResource>(intProjectId!.Value, request);
        });

        var files = items.Select(x => new FileEntity(x)).ToArray();
        return new(files);
    }

    [Action("Add file", Description = "Add new file")]
    public async Task<FileEntity> AddFile(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] AddNewFileRequest input)
    {
        try
        {
            var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
            var intBranchId = IntParser.Parse(input.BranchId, nameof(input.BranchId));
            var intDirectoryId = IntParser.Parse(input.DirectoryId, nameof(input.DirectoryId));
            var client = new CrowdinClient(Creds);

            var fileStream = await _fileManagementClient.DownloadAsync(input.File);
            var storage = await client.AddStorageAsync(input.File.Name, fileStream);

            var request = new AddFileRequestDto
            {
                StorageId = storage.Id,
                Name = input.File.Name,
                BranchId = intBranchId,
                DirectoryId = intDirectoryId,
                Title = input.Title,
                ExcludedTargetLanguages = input.ExcludedTargetLanguages?.ToList(),
                AttachLabelIds = input.AttachLabelIds?.ToList()
            };

            var file = await client.AddFileAsync(intProjectId!.Value, request);
            return new(file);
        }
        catch (Exception e)
        {
            await Logger.LogAsync(new
            {
                ExceptionMessage = e.Message,
                ExceptionStackTrace = e.StackTrace,
                ExceptionType = e.GetType().ToString(),
            });
            throw;
        }
    }

    [Action("Update file", Description = "Update an existing file with new content")]
    public async Task<FileEntity> UpdateFile(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] UpdateFileRequest input)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
        var intFileId = IntParser.Parse(input.FileId, nameof(input.FileId));

        var client = new CrowdinClient(Creds);

        var fileStream = await _fileManagementClient.DownloadAsync(input.File);
        var storage = await client.Storage.AddStorage(fileStream, input.File.Name);

        var (file, isModified) = await client.SourceFiles.UpdateOrRestoreFile(intProjectId!.Value, intFileId!.Value,
            new ReplaceFileRequest
            {
                StorageId = storage.Id,
                UpdateOption = ToOptionEnum(input.UpdateOption)
            });

        return new(file, isModified);
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
            return await UpdateFile(project, new()
                { File = input.File, FileId = existingFile.Id, UpdateOption = input.UpdateOption });
        }

        return await AddFile(project, input);
    }

    [Action("Get file", Description = "Get specific file info")]
    public async Task<FileEntity> GetFile(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] [Display("File ID")] string fileId)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
        var intFileId = IntParser.Parse(fileId, nameof(fileId));

        var client = new CrowdinClient(Creds);

        var file = await client.SourceFiles.GetFile<FileResource>(intProjectId!.Value, intFileId!.Value);
        return new(file);
    }

    [Action("Download file", Description = "Download specific file")]
    public async Task<DownloadFileResponse> DownloadFile(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] [Display("File ID")] string fileId)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
        var intFileId = IntParser.Parse(fileId, nameof(fileId));

        var client = new CrowdinClient(Creds);

        var fileInfo = await client.SourceFiles.GetFile<FileResource>(intProjectId!.Value, intFileId!.Value);

        var downloadLink = await client.SourceFiles.DownloadFile(intProjectId!.Value, intFileId!.Value);

        //temp fix https://dev.azure.com/blackbird-io/Blackbird.io/_workitems/edit/3397
        //if (!MimeTypes.TryGetMimeType(fileInfo.Name, out var contentType))
        string contentType = "application/octet-stream";

        var bytes = new RestClient(downloadLink.Url).Get(new RestRequest("/")).RawBytes;

        var file = await _fileManagementClient.UploadAsync(new MemoryStream(bytes), contentType, fileInfo.Name);
        return new(file);
    }

    [Action("Delete file", Description = "Delete specific file")]
    public Task DeleteFile(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] [Display("File ID")] string fileId)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
        var intFileId = IntParser.Parse(fileId, nameof(fileId));

        var client = new CrowdinClient(Creds);

        return client.SourceFiles.DeleteFile(intProjectId!.Value, intFileId!.Value);
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