using Newtonsoft.Json;

namespace Apps.Crowdin.Models.Response.Project
{
    public class ReportStatusResponse
    {
        [JsonProperty("data")]
        public ReportStatus Data { get; set; }
    }

    public class ReportStatus
    {
        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("progress")]
        public int Progress { get; set; }

    }
}
