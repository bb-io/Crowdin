using System.Net.Http.Headers;
using System.Net.Mime;
using Apps.Crowdin.Api;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.File;
using Apps.Crowdin.Models.Request.Project;
using Apps.Crowdin.Models.Response.File;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Parsers;
using Blackbird.Applications.Sdk.Utils.Utilities;
using Crowdin.Api.SourceFiles;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.Crowdin.Actions;

[ActionList]
public class FileActions : BaseInvocable
{
    private AuthenticationCredentialsProvider[] Creds =>
        InvocationContext.AuthenticationCredentialsProviders.ToArray();

    public FileActions(InvocationContext invocationContext) : base(invocationContext)
    {
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
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
        var intStorageId = IntParser.Parse(input.StorageId, nameof(input.StorageId));
        var intBranchId = IntParser.Parse(input.BranchId, nameof(input.BranchId));
        var intDirectoryId = IntParser.Parse(input.DirectoryId, nameof(input.DirectoryId));

        var client = new CrowdinClient(Creds);

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

        var downloadLink = await client.SourceFiles.DownloadFile(intProjectId!.Value, intFileId!.Value);
        var fileContent = await FileDownloader.DownloadFileBytes(downloadLink.Url);

        var result = new File(fileContent)
        {
            ContentType = MediaTypeNames.Application.Octet,
        };
        return new(result);
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
}