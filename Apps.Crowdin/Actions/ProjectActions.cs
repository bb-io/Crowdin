using Apps.Crowdin.Api;
using Apps.Crowdin.Constants;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.Project;
using Apps.Crowdin.Models.Response.Project;
using Apps.Crowdin.Utils;
using Apps.Crowdin.Utils.Parsers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Crowdin.Api.ProjectsGroups;
using AddProjectRequest = Crowdin.Api.ProjectsGroups.AddProjectRequest;

namespace Apps.Crowdin.Actions;

[ActionList]
public class ProjectActions
{
    [Action("List projects", Description = "List all projects")]
    public async Task<ListProjectsResponse> ListProjects(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] ListProjectsRequest input)
    {
        var userId = IntParser.Parse(input.UserId, nameof(input.UserId));
        var groupId = IntParser.Parse(input.GroupID, nameof(input.GroupID));
        
        var client = new CrowdinClient(creds);

        var items = await Paginator.Paginate((lim, offset)
            => client.ProjectsGroups.ListProjects<ProjectBase>(userId, groupId, input.HasManagerAccess ?? false, lim, offset));

        var projects = items.Select(x => new ProjectEntity(x)).ToArray();

        return new(projects);
    }
    
    [Action("Get project", Description = "Get specific project")]
    public async Task<ProjectEntity> GetProject(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] [Display("Project ID")] string projectId)
    {
        var intProjectId = IntParser.Parse(projectId, nameof(projectId));

        var client = new CrowdinClient(creds);

        var response = await client.ProjectsGroups.GetProject<ProjectBase>(intProjectId!.Value);
        return new(response);
    }

    [Action("Add project", Description = "Add new project")]
    public async Task<ProjectEntity> AddProject(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] AddNewProjectRequest input)
    {
        var client = new CrowdinClient(creds);
        
        var request = new StringsBasedProjectForm
        {
            Name = input.Name,
            SourceLanguageId = input.SourceLanguageId,
            Identifier = input.Identifier,
            Visibility = EnumParser.Parse<ProjectVisibility>(input.Visibility, nameof(input.Visibility), EnumValues.ProjectVisibility),
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
        
        var response = await client.ProjectsGroups.AddProject<ProjectBase>(request);
        return new(response);
    }

    [Action("Delete project", Description = "Delete specific project")]
    public Task DeleteProject(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] [Display("Project ID")] string projectId)
    {
        var intProjectId = IntParser.Parse(projectId, nameof(projectId));
        
        var client = new CrowdinClient(creds);

        return client.ProjectsGroups.DeleteProject(intProjectId!.Value);
    }
}