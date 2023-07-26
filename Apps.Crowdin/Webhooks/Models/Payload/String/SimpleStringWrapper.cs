using Apps.Crowdin.Webhooks.Models.Payload.Base.Wrapper;
using Newtonsoft.Json;

namespace Apps.Crowdin.Webhooks.Models.Payload.String;

public class SimpleStringWrapper : SimpleWebhookWrapper
{
    [JsonProperty("string_id")]
    public string StringId { get; set; }
    
    [JsonProperty("string_identifier")]
    public string StringIdentifier { get; set; }
    
    [JsonProperty("string_context")]
    public string StringContext { get; set; }
    public string User { get; set; }
    
    [JsonProperty("user_id")]
    public string UserId { get; set; }
    
    public string File { get; set; }
    
    [JsonProperty("file_id")]
    public string FileId { get; set; }
}