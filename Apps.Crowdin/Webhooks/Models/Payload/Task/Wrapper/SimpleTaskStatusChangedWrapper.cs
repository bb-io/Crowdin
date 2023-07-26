using Newtonsoft.Json;

namespace Apps.Crowdin.Webhooks.Models.Payload.Task.Wrapper;

public class SimpleTaskStatusChangedWrapper : SimpleTaskWrapper
{
    [JsonProperty("old_status")]
    public string OldStatus { get; set; }
    
    [JsonProperty("new_status")]
    public string NewStatus { get; set; }
}