using Apps.Crowdin.Api.RestSharp;
using Apps.Crowdin.Invocables;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Response;
using Apps.Crowdin.Models.Response.Project;
using Apps.Crowdin.Polling.Models;
using Apps.Crowdin.Polling.Models.Requests;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;
using Crowdin.Api;
using RestSharp;

namespace Apps.Crowdin.Polling;

[PollingEventList("Projects")]
public class ProjectPollingList(InvocationContext context) : AppInvocable(context)
{
    [PollingEvent("[Enterprise] On projects assigned to vendor", 
        Description = "Triggers when a client organization assigns a new project to your vendor organization")]
    public async Task<PollingEventResponse<ProjectsPollingMemory, ListProjectsResponse>> OnProjectAssignedToVendor(
        PollingEventRequest<ProjectsPollingMemory> pollingRequest,
        [PollingEventParameter] OnProjectAssignedToVendorRequest input)
    {
        CheckAccessToEnterpriseAction("event");
        
        var projects = await Paginator.Paginate(async (lim, offset) =>
        {
            var request = new CrowdinRestRequest("/projects", Method.Get, Creds); 
            
            if (!string.IsNullOrEmpty(input.GroupId))
                request.AddQueryParameter("groupId", input.GroupId);
            
            request.AddQueryParameter("limit", lim);
            request.AddQueryParameter("offset", offset);

            return await RestClient.ExecuteWithErrorHandling<ResponseList<DataResponse<ProjectEntity>>>(request);
        });
        
        var externalProjectIds = projects.Where(x => x.Data.IsExternal).Select(x => x.Data.Id).ToList();

        if (pollingRequest.Memory is null)
        {
            return new()
            {
                FlyBird = false,
                Memory = new() { KnownProjectIds = externalProjectIds }
            };
        }

        var newlyAssignedProjects = projects
            .Where(x => x.Data.IsExternal && !pollingRequest.Memory.KnownProjectIds.Contains(x.Data.Id))
            .Select(x => x.Data)
            .ToList();

        return new()
        {
            FlyBird = newlyAssignedProjects.Count > 0,
            Result = new(newlyAssignedProjects),
            Memory = new() { KnownProjectIds = newlyAssignedProjects.Count > 0 ? externalProjectIds : pollingRequest.Memory.KnownProjectIds }
        };
    }
}