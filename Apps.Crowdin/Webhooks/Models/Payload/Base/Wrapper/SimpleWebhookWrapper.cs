using Newtonsoft.Json;

namespace Apps.Crowdin.Webhooks.Models.Payload.Base.Wrapper;

public class SimpleWebhookWrapper
{
    public string Project { get; set; }

    [JsonProperty("project_id")] public string ProjectId { get; set; }
}