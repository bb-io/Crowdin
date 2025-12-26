using Apps.Crowdin.Models.Entities;

namespace Apps.Crowdin.Models.Response.Workflow
{
    public record FindWorkflowStepsResponse(WorkflowStepEntity[] WorkflowSteps, string[] WorkflowStepIds);
}
