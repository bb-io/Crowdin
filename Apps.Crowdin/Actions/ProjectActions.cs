using Apps.Crowdin.Api;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.Project;
using Apps.Crowdin.Models.Response.Project;
using Apps.Crowdin.Utils;
using Apps.Crowdin.Utils.Parsers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Crowdin.Api.ProjectsGroups;

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
    // TODO: add action using rest api directly
    // [Action("Add project", Description = "Add new project")]
    // public async Task<ProjectEntity> AddProject(
    //     IEnumerable<AuthenticationCredentialsProvider> creds,
    //     [ActionParameter] [Display("Project ID")] string projectId)
    // {
    //     var client = new CrowdinClient(creds);
    //     
    //     var request = new EnterpriseProject()
    //     {
    //
    //     };
    //     
    //     var response = await client.ProjectsGroups.AddProject<ProjectBase>(intProjectId!.Value);
    //     return new(response);
    // }

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