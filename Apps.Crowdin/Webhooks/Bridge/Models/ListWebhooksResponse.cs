using Newtonsoft.Json;

namespace Apps.Crowdin.Webhooks.Bridge.Models
{
    public class ListWebhooksResponse
    {
        [JsonProperty("data")]
        public List<WebhookResponseItem> Data { get; set; } = new List<WebhookResponseItem>();

        [JsonProperty("pagination")]
        public PaginationModel Pagination { get; set; } = new PaginationModel();
    }

    public class WebhookResponseItem
    {
        [JsonProperty("data")]
        public WebhookModel Data { get; set; } = new();
    }

    public class WebhookModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("url")]
        public string Url { get; set; } = string.Empty;

        [JsonProperty("events")]
        public List<string> Events { get; set; } = new List<string>();

        [JsonProperty("requestType")]
        public string RequestType { get; set; } = string.Empty;

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("batchingEnabled")]
        public bool BatchingEnabled { get; set; }

        [JsonProperty("contentType")]
        public string ContentType { get; set; } = string.Empty;

        [JsonProperty("headers")]
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();


        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }
    }
    public class PaginationModel
    {
        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }
    }
}
