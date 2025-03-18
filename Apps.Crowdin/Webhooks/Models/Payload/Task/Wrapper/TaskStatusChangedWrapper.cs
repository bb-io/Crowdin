using Newtonsoft.Json;

namespace Apps.Crowdin.Webhooks.Models.Payload.Task.Wrapper;

public class TaskStatusChangedWrapper
{
    [JsonProperty("task")]
    public TaskStatusChangedPayload Task { get; set; } = new();
}