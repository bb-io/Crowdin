using Newtonsoft.Json;

namespace Apps.Crowdin.Models.Dtos;

public class ErrorDto
{
    [JsonProperty("error")]
    public ErrorDetail Error { get; set; } = new();
}
