using Apps.Crowdin.Webhooks.Models.Payload.Base.Wrapper;
using Newtonsoft.Json;

namespace Apps.Crowdin.Webhooks.Models.Payload.Task.Wrapper;

public class SimpleTaskWrapper : SimpleWebhookWrapper
{
    public string Language { get; set; }
    
    [JsonProperty("task_id")]
    public string TaskId { get; set; }
    
    public string User { get; set; }
    
    [JsonProperty("user_id")]
    public string UserId { get; set; }
}