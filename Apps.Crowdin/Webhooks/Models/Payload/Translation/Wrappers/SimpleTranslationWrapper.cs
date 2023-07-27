using Apps.Crowdin.Webhooks.Models.Payload.Base.Wrapper;
using Newtonsoft.Json;

namespace Apps.Crowdin.Webhooks.Models.Payload.Translation.Wrappers;

public class SimpleTranslationWrapper : SimpleWebhookWrapper
{
    public string Language { get; set; }
    
    [JsonProperty("source_string_id")]
    public string SourceStringId { get; set; }
    
    [JsonProperty("old_translation_id")]
    public string OldTranslationId { get; set; }
    
    [JsonProperty("new_translation_id")]
    public string NewTranslationId { get; set; }
    
    public string User { get; set; }
    
    [JsonProperty("user_id")]
    public string UserId { get; set; }
    
    public string Provider { get; set; }
    
    [JsonProperty("is_pre_translated")]
    public bool IsPreTranslated { get; set; }
    
    [JsonProperty("file_id")]
    public string FileId { get; set; }
    
    public string File { get; set; }
}