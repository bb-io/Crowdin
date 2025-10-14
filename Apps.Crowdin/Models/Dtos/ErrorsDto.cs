using Newtonsoft.Json;

namespace Apps.Crowdin.Models.Dtos;

public class ErrorsDto
{
    [JsonProperty("key")]
    public string Key { get; set; } = string.Empty;

    [JsonProperty("errors")]
    public List<ErrorDetail> Errors { get; set; } = new();
}