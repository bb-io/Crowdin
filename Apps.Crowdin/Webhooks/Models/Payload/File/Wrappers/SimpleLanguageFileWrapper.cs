using Apps.Crowdin.Webhooks.Models.Payload.Base.Wrapper;
using Newtonsoft.Json;

namespace Apps.Crowdin.Webhooks.Models.Payload.File.Wrappers;

public class SimpleLanguageFileWrapper : SimpleWebhookWrapper
{
    [JsonProperty("file_id")] public string FileId { get; set; }

    public string File { get; set; }

    public string Language { get; set; }
}