using Apps.Crowdin.Api.RestSharp;
using Apps.Crowdin.Api.RestSharp.Enterprise;
using Apps.Crowdin.Invocables;
using Apps.Crowdin.Models.Dtos;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.Project;
using Apps.Crowdin.Models.Request.Workflow;
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
            }).ToArray();

        return new ListWorkflowStepsResponse(workflowSteps);
    }

    [Action("[Enterprise] Find workflow step", Description = "Find workflow step(s) in project by type (and optional title)")]
    public async Task<FindWorkflowStepsResponse> FindWorkflowStep(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] FindWorkflowStepRequest input)
    {
        CheckAccessToEnterpriseAction();

        var type = (input.Type ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Workflow step type is required.", nameof(input.Type));

        var steps = await GetWorkflowSteps(project);

        var filtered = steps
            .Where(x => !string.IsNullOrWhiteSpace(x.Type) &&
                        string.Equals(x.Type.Trim(), type, StringComparison.OrdinalIgnoreCase));

        var title = input.Title?.Trim();
        if (!string.IsNullOrWhiteSpace(title))
        {
            var contains = input.TitleContains.GetValueOrDefault(false);

            filtered = contains
                ? filtered.Where(x => !string.IsNullOrWhiteSpace(x.Title) &&
                                      x.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
                : filtered.Where(x => !string.IsNullOrWhiteSpace(x.Title) &&
                                      string.Equals(x.Title.Trim(), title, StringComparison.OrdinalIgnoreCase));
        }

        var resultSteps = filtered.ToArray();
        var ids = resultSteps.Select(x => x.Id).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

        return new FindWorkflowStepsResponse(resultSteps, ids);
    }

    private async Task<WorkflowStepEntity[]> GetWorkflowSteps(ProjectRequest project)
    {
        CheckAccessToEnterpriseAction();

        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId))!.Value;
        var enterpriseRestClient = new CrowdinEnterpriseRestClient(invocationContext.AuthenticationCredentialsProviders);

        var request = new CrowdinRestRequest(
            $"/projects/{intProjectId}/workflow-steps",
            Method.Get,
            invocationContext.AuthenticationCredentialsProviders);

        var response = await enterpriseRestClient.ExecuteWithErrorHandling(request);

        var workflowResponseDto = string.IsNullOrWhiteSpace(response.Content)
            ? null
            : JsonConvert.DeserializeObject<WorkflowStepsResponseDto>(response.Content);

        var workflowSteps = workflowResponseDto?.Data?
            .Select(wrapper => wrapper.Data)
            .Where(dto => dto != null)
            .Select(dto => new WorkflowStepEntity
            {
                Id = dto.Id.ToString(),
                Title = dto.Title,
                Type = dto.Type,
                Languages = dto.Languages
            })
            .ToArray();

        return workflowSteps ?? Array.Empty<WorkflowStepEntity>();
    }
}