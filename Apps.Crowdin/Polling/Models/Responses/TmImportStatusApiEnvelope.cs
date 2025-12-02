using Newtonsoft.Json;

namespace Apps.Crowdin.Polling.Models.Responses
{
    public class TmImportStatusApiEnvelope
    {
        [JsonProperty("data")]
        public TmImportStatusApiModel Data { get; set; } = default!;
    }

    public class TmImportStatusApiModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("identifier")]
        public string Identifier { get; set; } = default!;

        [JsonProperty("status")]
        public string Status { get; set; } = default!;

        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTimeOffset? UpdatedAt { get; set; }

        [JsonProperty("progress")]
        public int? Progress { get; set; }
    }
}
