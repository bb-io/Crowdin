using Apps.Crowdin.Invocables;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.Project;
using Apps.Crowdin.Models.Response.Workflow;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Parsers;
using Crowdin.Api.Workflows;

namespace Apps.Crowdin.Actions;

[ActionList]
public class WorkflowActions(InvocationContext invocationContext) : AppInvocable(invocationContext)
{
    [Action("[Enterprise] Search workflow steps", Description = "Search all workflow steps of project")]
    public async Task<ListWorkflowStepsResponse> ListWorkflowSteps([ActionParameter] ProjectRequest project)
    {
        CheckAccessToEnterpriseAction();
        
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId))!.Value;
        var executor = new WorkflowsApiExecutor(SdkClient);

        var response = await executor.ListWorkflowSteps(intProjectId);
        var workflowSteps = response.Data.Select(x => new WorkflowStepEntity(x)).ToArray();
        
        return new(workflowSteps);
    }
}