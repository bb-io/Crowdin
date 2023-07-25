using Apps.Crowdin.Webhooks.Models.Payload.Project.Response.Base;
using Apps.Crowdin.Webhooks.Models.Payload.Project.Wrappers;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Webhooks.Models.Payload.Project.Response;

public class ProjectBuiltWebhookResponse : ProjectWebhookResponse
{
    [Display("Build ID")]
    public string BuildId { get; set; }
    
    [Display("Build download URL")]
    public string BuildDownloadUrl { get; set; }

    public ProjectBuiltWebhookResponse(ProjectBuildWrapper wrapper) : base(wrapper.Build.Project)
    {
        BuildId = wrapper.Build.Id;
        BuildDownloadUrl = wrapper.Build.DownloadLink;
    }
}