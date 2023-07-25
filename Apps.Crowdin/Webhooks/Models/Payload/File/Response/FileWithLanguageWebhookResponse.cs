using Apps.Crowdin.Webhooks.Models.Payload.File.Response.Base;
using Apps.Crowdin.Webhooks.Models.Payload.File.Wrappers;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Webhooks.Models.Payload.File.Response;

public class FileWithLanguageWebhookResponse : FileWebhookResponse
{
    [Display("Target language ID")]
    public string LanguageId { get; set; }

    public FileWithLanguageWebhookResponse(FileWithLanguageWrapper wrapper) : base(wrapper.File)
    {
        LanguageId = wrapper.TargetLanguage.Id;
    }
}