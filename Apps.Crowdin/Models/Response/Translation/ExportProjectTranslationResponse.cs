
using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Crowdin.Models.Response.Translation
{
    public class ExportProjectTranslationResponse
    {
        [JsonProperty("data")]
        public ExportProjectTranslationData Data { get; set; }
    }

    public class ExportProjectTranslationData
    {
        [JsonProperty("url")]
        [Display("URL to download")]
        public string Url { get; set; }

        [JsonProperty("expireIn")]
        [Display("Expire in")]
        public string ExpireIn { get; set; }
    }
}
