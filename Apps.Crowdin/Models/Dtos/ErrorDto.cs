using Newtonsoft.Json;

namespace Apps.Crowdin.Models.Dtos;

public class ErrorDto
{
    [JsonProperty("key")]
    public string Key { get; set; } = string.Empty;

    [JsonProperty("errors")]
    public List<ErrorDetail> Errors { get; set; } = new();
}