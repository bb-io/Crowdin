using Newtonsoft.Json;
using System.Text.Json;

namespace Apps.Crowdin.Models.Response.Project
{
    public class GenerateReportResponse
    {
        [JsonProperty("data")]
        public ReportData Data { get; init; }

    }
    public sealed class ReportData
    {
        [JsonProperty("identifier")]
        public Guid Identifier { get; init; }

        [JsonProperty("status")]
        public string Status { get; init; }

        [JsonProperty("progress")]
        public int Progress { get; init; }

        [JsonProperty("attributes")]
        public ReportAttributes Attributes { get; init; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; init; }

        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; init; }

        [JsonProperty("startedAt")]
        public DateTime? StartedAt { get; init; }

        [JsonProperty("finishedAt")]
        public DateTime? FinishedAt { get; init; }
    }

    public sealed class ReportAttributes
    {
        [JsonProperty("format")]
        public string Format { get; init; }

        [JsonProperty("reportName")]
        public string ReportName { get; init; }

        [JsonProperty("schema")]
        public JsonElement Schema { get; init; }
    }
}
