using Newtonsoft.Json;

namespace Apps.Crowdin.Models.Dtos;

public class ErrorResponse
{
    [JsonProperty("errors")]
    public List<ErrorWrapper> Errors { get; set; } = new();
}