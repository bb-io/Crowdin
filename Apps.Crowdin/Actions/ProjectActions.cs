using Apps.Crowdin.Api;
using Apps.Crowdin.Invocables;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.Project;
using Apps.Crowdin.Models.Response.File;
using Apps.Crowdin.Models.Response.Project;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Blackbird.Applications.Sdk.Utils.Models;
using Blackbird.Applications.Sdk.Utils.Parsers;
using Blackbird.Applications.Sdk.Utils.Utilities;
using Crowdin.Api.ProjectsGroups;
using Crowdin.Api.Translations;

namespace Apps.Crowdin.Actions;

[ActionList]
public class ProjectActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : AppInvocable(invocationContext)
{
    [Action("Search projects", Description = "List all projects")]
    public async Task<ListProjectsResponse> ListProjects([ActionParameter] ListProjectsRequest input)
    {
        var userId = IntParser.Parse(input.UserId, nameof(input.UserId));
        var groupId = IntParser.Parse(input.GroupID, nameof(input.GroupID));

        var items = await Paginator.Paginate((lim, offset)
            => ExceptionWrapper.ExecuteWithErrorHandling(() =>
                SdkClient.ProjectsGroups.ListProjects<ProjectBase>(userId, groupId, input.HasManagerAccess ?? false,
                    null, lim, offset)));

        var projects = items.Select(x => new ProjectEntity(x)).ToArray();
        return new(projects);
    }

    [Action("Get project", Description = "Get specific project")]
    public async Task<ProjectEntity> GetProject([ActionParameter] ProjectRequest project)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
            await SdkClient.ProjectsGroups.GetProject<ProjectBase>(intProjectId!.Value));
        return new(response);
    }

    [Action("Add project", Description = "Add new project")]
    public async Task<ProjectEntity> AddProject([ActionParameter] AddNewProjectRequest input)
    {
        var request = new StringsBasedProjectForm
        {
            Name = input.Name,
            SourceLanguageId = input.SourceLanguageId,
            Identifier = input.Identifier,
            Visibility = EnumParser.Parse<ProjectVisibility>(input.Visibility, nameof(input.Visibility)),
            TargetLanguageIds = input.TargetLanguageIds?.ToList(),
            Cname = input.Cname,
            Description = input.Description,
            IsMtAllowed = input.IsMtAllowed,
            AutoSubstitution = input.AutoSubstitution,
            AutoTranslateDialects = input.AutoTranslateDialects,
            PublicDownloads = input.PublicDownloads,
            HiddenStringsProofreadersAccess = input.HiddenStringsProofreadersAccess,
            UseGlobalTm = input.UseGlobalTm,
            SkipUntranslatedStrings = input.SkipUntranslatedStrings,
            SkipUntranslatedFiles = input.SkipUntranslatedFiles,
            ExportApprovedOnly = input.ExportApprovedOnly,
            InContext = input.InContext,
            InContextProcessHiddenStrings = input.InContextProcessHiddenStrings,
            InContextPseudoLanguageId = input.InContextPseudoLanguageId,
            QaCheckIsActive = input.QaCheckIsActive,
        };

        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
            await SdkClient.ProjectsGroups.AddProject<ProjectBase>(request));
        return new(response);
    }

    [Action("Delete project", Description = "Delete specific project")]
    public async Task DeleteProject([ActionParameter] ProjectRequest project)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
        await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
            await SdkClient.ProjectsGroups.DeleteProject(intProjectId!.Value));
    }

    [Action("Build project", Description = "Build project translation")]
    public async Task<ProjectBuildEntity> BuildProject([ActionParameter] ProjectRequest project,
        [ActionParameter] BuildProjectRequest input)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));

        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
            await SdkClient.Translations.BuildProjectTranslation(intProjectId!.Value,
                new EnterpriseTranslationCreateProjectBuildForm
                {
                    BranchId = IntParser.Parse(input.BranchId, nameof(input.BranchId)),
                    TargetLanguageIds = input.TargetLanguageIds?.ToList(),
                    SkipUntranslatedStrings = input.SkipUntranslatedStrings,
                    SkipUntranslatedFiles = input.SkipUntranslatedFiles,
                    ExportWithMinApprovalsCount = input.ExportWithMinApprovalsCount
                }));

        return new(response);
    }

    [Action("Download translations as ZIP", Description = "Download project translations as ZIP")]
    public async Task<DownloadFileResponse> DownloadTranslationsAsZip(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] BuildRequest build)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId))!.Value;
        var intBuildId = IntParser.Parse(build.BuildId, nameof(build.BuildId))!.Value;

        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () => 
            await SdkClient.Translations.DownloadProjectTranslations(intProjectId, intBuildId));

        if (response.Link is null)
        {
            throw new PluginMisconfigurationException("Project build is in progress, you can't download the data now");
        }

        var file = await ExceptionWrapper.ExecuteWithErrorHandling(() => FileDownloader.DownloadFileBytes(response.Link.Url));
        await FileOperationWrapper.ExecuteFileOperation(() => Task.CompletedTask, file.FileStream, file.Name);

        file.Name = $"{project.ProjectId}.zip";

        var memoryStream = new MemoryStream();
        await file.FileStream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        var fileReference = await fileManagementClient.UploadAsync(memoryStream, file.ContentType, file.Name);
        return new(fileReference);
    }

    [Action("Download translations", Description = "Download project translations")]
    public async Task<DownloadFilesResponse> DownloadTranslations(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] BuildRequest build)
    {
        var filesArchive = await DownloadTranslationsAsZip(project, build);

        var zipFile = await FileOperationWrapper.ExecuteFileDownloadOperation(() => fileManagementClient.DownloadAsync(filesArchive.File), filesArchive.File.Name);
        var zipBytes = await zipFile.GetByteData();
        var files = await zipFile.GetFilesFromZip();

        var result = files.Where(x => x.FileStream.Length > 0).ToArray();

        // Cleaning files path from the root folder of the archive
        result.ToList().ForEach(x =>
            x.Path = string.Join('/', x.Path.Split("/").Skip(1)));

        var tasks = result.Select(x =>
        {
            var contentType = MimeTypes.GetMimeType(x.UploadName);
            return fileManagementClient.UploadAsync(x.FileStream, contentType, x.UploadName);
        }).ToList();

        var fileReferences = await Task.WhenAll(tasks);
        return new(fileReferences.ToList());
    }
}