using Apps.Crowdin.Models.Entities;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Response.Workflow;

public record ListWorkflowStepsResponse([property: Display("Workflow steps")] 
    WorkflowStepEntity[] WorkflowSteps);