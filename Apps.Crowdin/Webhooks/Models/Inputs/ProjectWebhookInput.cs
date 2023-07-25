using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Webhooks.Models.Inputs;

public class ProjectWebhookInput
{
    [Display("Project ID")]
    public string ProjectId { get; set; }
}