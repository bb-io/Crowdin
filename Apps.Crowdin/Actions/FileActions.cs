using Apps.Crowdin.Api;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.File;
using Apps.Crowdin.Models.Response.File;
using Apps.Crowdin.Utils;
using Apps.Crowdin.Utils.Parsers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Crowdin.Api.SourceFiles;

namespace Apps.Crowdin.Actions;

[ActionList]
public class FileActions
{
    [Action("List files", Description = "List project files")]
    public async Task<ListFilesResponse> ListFiles(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] [Display("Project ID")]
        string projectId,
        [ActionParameter] ListFilesRequest input)
    {
        var intProjectId = IntParser.Parse(projectId, nameof(projectId));
        var intBranchId = IntParser.Parse(input.BranchId, nameof(input.BranchId));
        var intDirectoryId = IntParser.Parse(input.DirectoryId, nameof(input.DirectoryId));

        var client = new CrowdinClient(creds);

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
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] [Display("Project ID")] string projectId,
        [ActionParameter] AddNewFileRequest input)
    {
        var intProjectId = IntParser.Parse(projectId, nameof(projectId));
        var intStorageId = IntParser.Parse(input.StorageId, nameof(input.StorageId));
        var intBranchId = IntParser.Parse(input.BranchId, nameof(input.BranchId));
        var intDirectoryId = IntParser.Parse(input.DirectoryId, nameof(input.DirectoryId));

        var client = new CrowdinClient(creds);

        var request = new AddFileRequest
        {
            StorageId = intStorageId!.Value,
            Name = input.Name,
            BranchId = intBranchId,
            DirectoryId = intDirectoryId,
            Title = input.Title,
            ExcludedTargetLanguages = input.ExcludedTargetLanguages?.ToList(),
            AttachLabelIds = input.AttachLabelIds?.ToList()
        };
        var file = await client.SourceFiles.AddFile(intProjectId!.Value, request);

        return new(file);
    }
    
    [Action("Get file", Description = "Get specific file info")]
    public async Task<FileEntity> GetFile(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] [Display("Project ID")] string projectId,
        [ActionParameter] [Display("File ID")] string fileId)
    {
        var intProjectId = IntParser.Parse(projectId, nameof(projectId));
        var intFileId = IntParser.Parse(fileId, nameof(fileId));

        var client = new CrowdinClient(creds);

        var file = await client.SourceFiles.GetFile<FileResource>(intProjectId!.Value, intFileId!.Value);
        return new(file);
    }      
    
    [Action("Download file", Description = "Download specific file")]
    public async Task<DownloadFileResponse> DownloadFile(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] [Display("Project ID")] string projectId,
        [ActionParameter] [Display("File ID")] string fileId)
    {
        var intProjectId = IntParser.Parse(projectId, nameof(projectId));
        var intFileId = IntParser.Parse(fileId, nameof(fileId));

        var client = new CrowdinClient(creds);

        var downloadLink = await client.SourceFiles.DownloadFile(intProjectId!.Value, intFileId!.Value);
        var fileContent = await FileDownloader.DownloadFileBytes(downloadLink.Url);
        
        return new(fileContent);
    }   
    
    [Action("Delete file", Description = "Delete specific file")]
    public Task DeleteFile(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] [Display("Project ID")] string projectId,
        [ActionParameter] [Display("File ID")] string fileId)
    {
        var intProjectId = IntParser.Parse(projectId, nameof(projectId));
        var intFileId = IntParser.Parse(fileId, nameof(fileId));

        var client = new CrowdinClient(creds);

        return client.SourceFiles.DeleteFile(intProjectId!.Value, intFileId!.Value);
    }
}