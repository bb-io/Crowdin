using Apps.Crowdin.Invocables;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.Project;
using Apps.Crowdin.Models.Response.File;
using Apps.Crowdin.Models.Response.Project;
using Apps.Crowdin.Models.Response.ReviewedFile;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Blackbird.Applications.Sdk.Utils.Parsers;
using Blackbird.Applications.Sdk.Utils.Utilities;

namespace Apps.Crowdin.Actions;

[ActionList]
public class ReviewedFileActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : AppInvocable(invocationContext)
{
    [Action("Search reviewed source files builds", Description = "List all reviewed source files builds of specific project")]
    public async Task<ListReviewedFileBuildsResponse> ListFileBuilds([ActionParameter] ProjectRequest project,
        [ActionParameter] [Display("Branch ID")] string? branchId)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId))!.Value;
        var intBranchId = IntParser.Parse(branchId, nameof(branchId));
        
        var items = await Paginator.Paginate((lim, offset) =>
            SdkClient.SourceFiles.ListReviewedSourceFilesBuilds(intProjectId, intBranchId, lim, offset));

        var result = items.Select(x => new ReviewedFileBuildEntity(x)).ToArray();
        return new(result);
    }  
    
    [Action("Build reviewed source files", Description = "Build reviewed source files of specific project")]
    public async Task<ReviewedFileBuildEntity> BuildReviewedSourceFiles([ActionParameter] ProjectRequest project,
        [ActionParameter] [Display("Branch ID")]
        string? branchId)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId))!.Value;
        var intBranchId = IntParser.Parse(branchId, nameof(branchId));
        
        var response = await SdkClient.SourceFiles.BuildReviewedSourceFiles(intProjectId, new()
        {
            BranchId = intBranchId
        });

        return new(response);
    }
    
    [Action("Get reviewed source files build", Description = "Get specific reviewed source files build")]
    public async Task<ReviewedFileBuildEntity> GetReviewedSourceFilesBuild([ActionParameter] ProjectRequest project,
        [ActionParameter] [Display("Build ID")]
        string buildId)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId))!.Value;
        var intBuildId = IntParser.Parse(buildId, nameof(buildId))!.Value;
        var response = await SdkClient.SourceFiles.CheckReviewedSourceFilesBuildStatus(intProjectId, intBuildId);
        return new(response);
    }
    
    [Action("Download reviewed source files as ZIP", Description = "Download reviewed source files of specific build as ZIP")]
    public async Task<DownloadFileResponse> DownloadReviewedSourceFilesAsZip([ActionParameter] ProjectRequest project,
        [ActionParameter] [Display("Build ID")]
        string buildId)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId))!.Value;
        var intBuildId = IntParser.Parse(buildId, nameof(buildId))!.Value;
        
        var response = await SdkClient.SourceFiles.DownloadReviewedSourceFiles(intProjectId, intBuildId);
        
        var file = await FileDownloader.DownloadFileBytes(response.Url);
        file.Name = $"{buildId}.zip";
        
        var fileReference = await fileManagementClient.UploadAsync(file.FileStream, file.ContentType, file.Name);
        return new(fileReference);
    }
    
    [Action("Download reviewed source files", Description = "Download reviewed source files of specific build")]
    public async Task<DownloadFilesResponse> DownloadReviewedSourceFiles([ActionParameter] ProjectRequest project,
        [ActionParameter] [Display("Build ID")]
        string buildId)
    {
        var zip = await DownloadReviewedSourceFilesAsZip(project, buildId);

        var zipFile = await fileManagementClient.DownloadAsync(zip.File);
        var zipBytes = await zipFile.GetByteData();
        var files = await zipFile.GetFilesFromZip();

        var tasks = files.Select(x =>
        {
            var contentType = MimeTypes.GetMimeType(x.UploadName);
            return fileManagementClient.UploadAsync(x.FileStream, contentType, x.UploadName);
        }).ToList();

        var fileReferences = await Task.WhenAll(tasks);
        return new(fileReferences.ToList());
    }
}