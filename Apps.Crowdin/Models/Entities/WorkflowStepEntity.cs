using Blackbird.Applications.Sdk.Common;
using Crowdin.Api.Workflows;

namespace Apps.Crowdin.Models.Entities;

public class WorkflowStepEntity
{
    [Display("Workflow step ID")]
    public string Id { get; set; }

    public string Title { get; set; }

    public string Type { get; set; }

    public string[] Languages { get; set; }
}