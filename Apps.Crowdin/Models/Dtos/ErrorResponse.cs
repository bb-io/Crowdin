using Newtonsoft.Json;

namespace Apps.Crowdin.Models.Dtos;

public class ErrorResponse
{
    [JsonProperty("error")]
    public ErrorDto Error { get; set; } = new();
}