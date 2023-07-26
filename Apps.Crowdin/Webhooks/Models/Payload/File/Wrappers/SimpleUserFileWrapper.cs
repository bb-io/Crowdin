using Apps.Crowdin.Webhooks.Models.Payload.Base.Wrapper;
using Newtonsoft.Json;

namespace Apps.Crowdin.Webhooks.Models.Payload.File.Wrappers;

public class SimpleUserFileWrapper : SimpleWebhookWrapper
{
    [JsonProperty("file_id")] public string FileId { get; set; }

    public string File { get; set; }

    [JsonProperty("user_id")] public string UserId { get; set; }

    public string User { get; set; }
}