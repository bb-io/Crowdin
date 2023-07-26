using Apps.Crowdin.Webhooks.Models.Payload.Base.Wrapper;

namespace Apps.Crowdin.Webhooks.Models.Payload.Project.Wrappers;

public class SimpleProjectWrapper : SimpleWebhookWrapper
{
    public string Language { get; set; }
}