using Apps.Crowdin.DataSourceHandlers;
using Apps.Crowdin.Invocables;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.ProjectGroups;
using Apps.Crowdin.Models.Response.ProjectGroups;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Parsers;

namespace Apps.Crowdin.Actions;

[ActionList("Project groups")]
public class ProjectGroupActions(InvocationContext invocationContext) : AppInvocable(invocationContext)
{
    [Action("[Enterprise] Search project groups", Description = "List all project groups")]
    public async Task<ListGroupsResponse> ListProjectGroups(
        [ActionParameter] [Display("Parent group ID")] [DataSource(typeof(ProjectGroupDataHandler))]
        string? parentId)
    {
        CheckAccessToEnterpriseAction();
        
        var intParentId = IntParser.Parse(parentId, nameof(parentId));
        var response = await Paginator.Paginate((lim, offset) =>
            ExceptionWrapper.ExecuteWithErrorHandling(() => SdkClient.ProjectsGroups.ListGroups(intParentId, lim, offset)));

        var groups = response.Select(x => new GroupEntity(x)).ToArray();
        return new(groups);
    }

    [Action("[Enterprise] Get project group", Description = "Get specific project group")]
    public async Task<GroupEntity> GetProjectGroup(
        [ActionParameter] ProjectGroupRequest group)
    {
        CheckAccessToEnterpriseAction();
        
        var intGroupId = IntParser.Parse(group.ProjectGroupId, nameof(group.ProjectGroupId))!.Value;
        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () => await SdkClient.ProjectsGroups.GetGroup(intGroupId));
        return new(response);
    }

    [Action("[Enterprise] Add project group", Description = "Add a new project group")]
    public async Task<GroupEntity> AddProjectGroup(
        [ActionParameter] AddProjectGroupRequest input)
    {
        CheckAccessToEnterpriseAction();
        
        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () => await SdkClient.ProjectsGroups.AddGroup(new()
        {
            Name = input.Name,
            Description = input.Description,
            ParentId = IntParser.Parse(input.ParentId, nameof(input.ParentId))
        }));
        
        return new(response);
    }

    [Action("[Enterprise] Delete project group", Description = "Delete specific project group")]
    public async Task DeleteProjectGroup(
        [ActionParameter] ProjectGroupRequest group)
    {
        CheckAccessToEnterpriseAction();
        await ExceptionWrapper.ExecuteWithErrorHandling(async () => 
            await SdkClient.ProjectsGroups.DeleteGroup(IntParser.Parse(group.ProjectGroupId, nameof(group.ProjectGroupId))!.Value));
    }
}