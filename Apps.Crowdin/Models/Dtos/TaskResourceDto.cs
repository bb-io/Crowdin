using Crowdin.Api.Tasks;
using Newtonsoft.Json;

namespace Apps.Crowdin.Models.Dtos;

public class TaskResourceDto
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("projectId")]
    public int ProjectId { get; set; }

    [JsonProperty("creatorId")]
    public int CreatorId { get; set; }

    [JsonProperty("vendor")]
    public string Vendor { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("assignees")]
    public TaskAssignee[] Assignees { get; set; }

    [JsonProperty("fileIds")]
    public int[] FileIds { get; set; }

    [JsonProperty("sourceLanguageId")]
    public string SourceLanguageId { get; set; }

    [JsonProperty("targetLanguageId")]
    public string TargetLanguageId { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("deadline")]
    public DateTime? DeadLine { get; set; }

    [JsonProperty("createdAt")]
    public DateTime CreatedAt { get; set; }
}
