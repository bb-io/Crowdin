using Newtonsoft.Json;

namespace Apps.Crowdin.Webhooks.Models.Payload.Task.Wrapper;

public class TaskStatusChangedWrapper
{
    [JsonProperty("data")]
    public TaskStatusChangedPayload Task { get; set; }
}