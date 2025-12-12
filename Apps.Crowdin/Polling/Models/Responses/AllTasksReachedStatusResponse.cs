using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Crowdin.Polling.Models.Responses
{
    public class AllTasksReachedStatusResponse
    {
        [Display("Tasks")]
        public List<TaskResource> Tasks { get; set; } = new();

        [Display("Task IDs")]
        public List<string> TaskIds { get; set; } = new();
    }

    public class ListResponse<T>
    {
        [JsonProperty("data")]
        public List<CrowdinData<T>> Data { get; set; } = new();

        [JsonProperty("pagination")]
        public CrowdinPagination? Pagination { get; set; }
    }

    public class CrowdinData<T>
    {
        [JsonProperty("data")]
        public T Data { get; set; } = default!;
    }

    public class CrowdinPagination
    {
        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }
    }

    public class TaskResource
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("projectId")]
        public long? ProjectId { get; set; }

        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }
}
