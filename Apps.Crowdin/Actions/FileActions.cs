using System.IO.Pipelines;
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
            await SdkClient.SourceFiles.GetFile<FileInfoResource>(intProjectId!.Value, intFileId!.Value));
        if (file is FileResource fileRes)
        {
            return new FileEntity(fileRes);
        }
        else
        {
            return new FileEntity(file);
        }
    }

    [Action("Add file", Description = "Add new file")]
    public async Task<FileEntity> AddFile(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] AddNewFileRequest input)
    {
        var fileName = input.Name ?? input.File.Name;
        if (!FileOperationWrapper.IsOnlyAscii(fileName))
        {
            throw new PluginMisconfigurationException(
                $"The file name '{fileName}' contains non-ASCII characters. " +
                "Crowdin API requires ASCII-only characters. Please rename the file and try again.");
        }

        if (string.IsNullOrEmpty(project.ProjectId))
        {
            throw new PluginMisconfigurationException("Project ID is null or empty. Please specify a valid ID");
        }
        
        if (input.StorageId is null && input.File is null)
        {
            throw new PluginMisconfigurationException("You need to specify one of the parameters: Storage ID or File");
        }

        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
        var intStorageId = LongParser.Parse(input.StorageId, nameof(input.StorageId));
        var intBranchId = IntParser.Parse(input.BranchId, nameof(input.BranchId));
        var intDirectoryId = IntParser.Parse(input.DirectoryId, nameof(input.DirectoryId));
        
        FileOperationWrapper.ValidateFileName(fileName);
        if (intStorageId is null && input.File != null)
        {
            var fileStream = await FileOperationWrapper.ExecuteFileDownloadOperation(() => fileManagementClient.DownloadAsync(input.File), input.File.Name);
            var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            
            var storage = await FileOperationWrapper.ExecuteFileOperation(() => SdkClient.Storage.AddStorage(memoryStream, fileName), memoryStream, fileName);
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
            
            if (ex.Message.Contains("New-line characters are not allowed in header values"))
            {
                throw new PluginMisconfigurationException(
                    "New-line characters are not allowed in header values. " +
                    "Please remove any line breaks (\\n or \\r) from the file name and try again."
                );
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
            var fileStream = await FileOperationWrapper.ExecuteFileDownloadOperation(() => fileManagementClient.DownloadAsync(input.File), input.File.Name);
            var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            
            var storage = await FileOperationWrapper.ExecuteFileOperation(() => client.Storage.AddStorage(memoryStream, input.File.Name), memoryStream, input.File.Name);
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
        var fileContent = await ExceptionWrapper.ExecuteWithErrorHandling(() => FileDownloader.DownloadFileBytes(downloadLink.Url));


        fileContent.Name = fileInfo.Name;
        fileContent.ContentType = fileContent.ContentType == MediaTypeNames.Text.Plain
            ? MediaTypeNames.Application.Octet
            : fileContent.ContentType;

        var fileReference = await ExceptionWrapper.ExecuteWithErrorHandling(() =>
            fileManagementClient.UploadAsync(fileContent.FileStream, fileContent.ContentType, fileContent.Name));
        return new DownloadFileResponse(fileReference);
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
        if (!int.TryParse(project.ProjectId, out var intProjectId))
            throw new PluginMisconfigurationException($"Invalid Project ID: {project.ProjectId} must be a numeric value. Please check the input project ID");

        if (!int.TryParse(fileId, out var intFileId))
            throw new PluginMisconfigurationException($"Invalid File ID: {fileId} must be a numeric value. Please check the input file ID");

        await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
        await SdkClient.SourceFiles.DeleteFile(intProjectId, intFileId));
    }

    [Action("Add spreadsheet file", Description = "Add a new spreadsheet (.csv or .xlsx) to Crowdin with optional spreadsheet settings")]
    public async Task<FileEntity> AddSpreadsheetFile(
    [ActionParameter] ProjectRequest project,
    [ActionParameter] AddNewSpreadsheetFileRequest input)
    {
        var fileName = input.Name ?? input.File?.Name;
        if (!FileOperationWrapper.IsOnlyAscii(fileName))
        {
            throw new PluginMisconfigurationException(
                $"The file name '{fileName}' contains non-ASCII characters. " +
                "Crowdin API requires ASCII-only characters. Please rename the file and try again.");
        }
        if (string.IsNullOrEmpty(project.ProjectId))
            throw new PluginMisconfigurationException("Project ID is null or empty. Please specify a valid ID");
        if (input.StorageId is null && input.File is null)
            throw new PluginMisconfigurationException("You must specify either Storage ID or File");

        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
        var intStorageId = LongParser.Parse(input.StorageId, nameof(input.StorageId));
        var intBranchId = IntParser.Parse(input.BranchId, nameof(input.BranchId));
        var intDirectoryId = IntParser.Parse(input.DirectoryId, nameof(input.DirectoryId));

        input.LanguageCodes ??= new[] { "en-US" };
        FileOperationWrapper.ValidateFileName(fileName);

        if (intStorageId is null && input.File != null)
        {
            var fileStream = await FileOperationWrapper.ExecuteFileDownloadOperation(
                () => fileManagementClient.DownloadAsync(input.File),
                input.File.Name);

            using var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            var storage = await FileOperationWrapper.ExecuteFileOperation(
                () => SdkClient.Storage.AddStorage(memoryStream, fileName!),
                memoryStream, fileName);

            intStorageId = storage.Id;
        }
        else if (input.File is null)
        {
            var storage = await new StorageActions(InvocationContext, fileManagementClient)
                .GetStorage(new() { StorageId = intStorageId.ToString()! });
            fileName = storage.FileName;
        }


        string extension = Path.GetExtension(fileName).ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(extension) && input.File?.ContentType is { } cType)
        {
            cType = cType.ToLowerInvariant();
            if (cType.Contains("csv"))
                extension = ".csv";
            else if (cType.Contains("excel") || cType.Contains("spreadsheet") || cType.Contains("xlsx"))
                extension = ".xlsx";
        }
        if (extension != ".csv" && extension != ".xlsx")
            throw new PluginMisconfigurationException("Only .csv and .xlsx files are supported by this action.");
        if (!fileName.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
            fileName += extension;

        ProjectFileType fileType;
        if (extension == ".csv")
        {
            fileType = ProjectFileType.Csv;
        }
        else
        {
            fileType = input.ImportEachCellAsSeparateSourceString == true ? ProjectFileType.DocX : ProjectFileType.Auto;
        }

        var scheme = new Dictionary<string, int?>();

        if (input.ContextColumnNumber.HasValue)
            scheme["context"] = input.ContextColumnNumber.Value;

        if (input.LanguageColumnNumbers == null || !input.LanguageColumnNumbers.Any())
        {
            if (!input.SourcePhraseColumnNumber.HasValue || !input.TranslationColumnNumber.HasValue)
                throw new PluginMisconfigurationException("For single language file, both Source phrase columnr and Translation column must be provided.");

            scheme["sourcePhrase"] = input.SourcePhraseColumnNumber.Value;
            scheme["translation"] = input.TranslationColumnNumber.Value;
        }
        else
        {
            var codes = input.LanguageCodes.ToArray();
            var cols = input.LanguageColumnNumbers.ToArray();
            if (codes.Length == 0 || cols.Length == 0 || codes.Length != cols.Length)
                throw new PluginMisconfigurationException("LanguageCodes and LanguageColumnNumbers must be provided with equal non-zero lengths.");

            scheme["sourcePhrase"] = input.SourcePhraseColumnNumber.Value;
            for (int i = 1; i < codes.Length; i++)
            {
                scheme[codes[i]] = cols[i];
            }
        }

        var importOptions = new Apps.Crowdin.Models.Request.File.CustomFileImportOptions
        {
            FirstLineContainsHeader = input.FirstLineContainsHeader,
            ImportTranslations = input.ImportTranslations,
            ImportHiddenSheets = input.ImportHiddenSheets,
            ContentSegmentation = input.ContentSegmentation,
            SrxStorageId = input.SrxStorageId,
            Scheme = scheme.ToDictionary(k => k.Key, k => k.Value)
        };

        var addFileRequest = new AddFileRequest
        {
            StorageId = intStorageId!.Value,
            Name = fileName!,
            BranchId = intBranchId,
            DirectoryId = intDirectoryId,
            Title = input.Title,
            ExcludedTargetLanguages = input.ExcludedTargetLanguages?.ToList(),
            AttachLabelIds = input.AttachLabelIds?.ToList(),
            Context = input.Context,
            Type = fileType,
            ImportOptions = importOptions
        };

        try
        {
            var newFile = await SdkClient.SourceFiles.AddFile(intProjectId!.Value, addFileRequest);
            return new FileEntity(newFile);
        }
        catch (CrowdinApiException ex)
        {
            if (!ex.Message.Contains("Name must be unique"))
                throw new PluginMisconfigurationException(ex.Message);

            var allFiles = await ListFiles(project, new());
            var fileToUpdate = allFiles.Files.First(x => x.Name == fileName);
            return await UpdateFile(
                project,
                new() { FileId = fileToUpdate.Id },
                new() { StorageId = intStorageId.ToString() },
                new());
        }

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