using Apps.Crowdin.Api.RestSharp;
using Apps.Crowdin.Api.RestSharp.Enterprise;
using Apps.Crowdin.Invocables;
using Apps.Crowdin.Models.Dtos;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.Project;
using Apps.Crowdin.Models.Response.Workflow;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Parsers;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Crowdin.Actions;

[ActionList("Workflows")]
public class WorkflowActions(InvocationContext invocationContext) : AppInvocable(invocationContext)
{
    [Action("[Enterprise] Search workflow steps", Description = "Search all workflow steps of project")]
    public async Task<ListWorkflowStepsResponse> ListWorkflowSteps([ActionParameter] ProjectRequest project)
    {
        CheckAccessToEnterpriseAction();

        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId))!.Value;
        var enterpriseRestClient = new CrowdinEnterpriseRestClient(invocationContext.AuthenticationCredentialsProviders);
        var request = new CrowdinRestRequest($"/projects/{intProjectId}/workflow-steps", Method.Get, invocationContext.AuthenticationCredentialsProviders);


        var response = await enterpriseRestClient.ExecuteWithErrorHandling(request);
        var workflowResponseDto = JsonConvert.DeserializeObject<WorkflowStepsResponseDto>(response.Content);

        var workflowSteps = workflowResponseDto?.Data
    .Select(wrapper => wrapper.Data) 
    .Select(dto => new WorkflowStepEntity
    {
        Id = dto.Id.ToString(),
        Title = dto.Title,
        Type = dto.Type,
        Languages = dto.Languages
    })
    .ToArray();

        return new ListWorkflowStepsResponse(workflowSteps);
    }
}