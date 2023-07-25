using Apps.Crowdin.Webhooks.Models.Payload.Project.Response.Base;
using Apps.Crowdin.Webhooks.Models.Payload.Project.Wrappers;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Webhooks.Models.Payload.Project.Response;

public class ProjectWithLanguageWebhookResponse : ProjectWebhookResponse
{
    [Display("Target language ID")] public string LanguageId { get; set; }

    public ProjectWithLanguageWebhookResponse(ProjectWithLanguageWrapper wrapper) : base(wrapper.Project)
    {
        LanguageId = wrapper.TargetLanguage.Id;
    }
}