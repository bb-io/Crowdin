using Apps.Crowdin.Webhooks.Models.Payload.Base.Wrapper;
using Newtonsoft.Json;

namespace Apps.Crowdin.Webhooks.Models.Payload.Suggestion.Wrappers;

public class SimpleSuggestionWrapper : SimpleWebhookWrapper
{
    public string Language { get; set; }
    
    [JsonProperty("source_string_id")] 
    public string SourceStringId { get; set; }
    
    [JsonProperty("translation_id")] 
    public string TranslationId { get; set; }
    
    public string User { get; set; }
    
    [JsonProperty("user_id")] 
    public string UserId { get; set; }
    
    public string Provider { get; set; }
    
    [JsonProperty("is_pre_translated")]
    public bool IsPreTranslated { get; set; }
    
    [JsonProperty("file_id")] 
    public bool FileId { get; set; }
    
    public bool File { get; set; }
}