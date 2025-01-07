using Blackbird.Applications.Sdk.Common;
using Crowdin.Api.Workflows;

namespace Apps.Crowdin.Models.Entities;

public class WorkflowStepEntity(WorkflowStep workflowStep)
{
    [Display("Workflow step ID")]
    public string Id { get; set; } = workflowStep.Id.ToString();
    
    public string Title { get; set; } = workflowStep.Title;
    
    public string Type { get; set; } = workflowStep.Type.ToString();
    
    public string[] Languages { get; set; } = workflowStep.Languages;
}