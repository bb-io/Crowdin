using Newtonsoft.Json;

namespace Apps.Crowdin.Models.Response.Branch
{
    public class BranchEntity
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("projectId")]
        public int ProjectId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("title")]
        public string Title { get; set; } = string.Empty;

        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("exportPattern")]
        public string ExportPattern { get; set; } = string.Empty;

        [JsonProperty("priority")]
        public string Priority { get; set; } = string.Empty;
    }
}
