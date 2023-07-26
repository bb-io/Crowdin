using Apps.Crowdin.Webhooks.Models.Payload.Base.Wrapper;
using Newtonsoft.Json;

namespace Apps.Crowdin.Webhooks.Models.Payload.Project.Wrappers;

public class SimpleProjectBuildWrapper : SimpleWebhookWrapper
{
    [JsonProperty("project_build_id")]
    public string ProjectBuildId { get; set; }
    
    [JsonProperty("project_build_download")]
    public string ProjectBuildIdDownloadUrl { get; set; }
}