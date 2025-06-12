using Newtonsoft.Json;

namespace Apps.Crowdin.Models.Response.Project
{
    public class GetReportResponse
    {
        [JsonProperty("data")]
        public DownloadData Data { get; init; }
    }
    public sealed class DownloadData
    {
        [JsonProperty("url")]
        public string Url { get; init; } = default!;

        [JsonProperty("expireIn")]
        public DateTimeOffset ExpireIn { get; init; }
    }
}
