using Newtonsoft.Json;

namespace Apps.Crowdin.Models.Dtos;

public class ErrorDto
{
    [JsonProperty("message")]
    public string Message { get; set; } = string.Empty;

    [JsonProperty("code")]
    public string Code { get; set; } = string.Empty;
}