using Newtonsoft.Json;

namespace Apps.Crowdin.Models.Dtos;
public class ErrorDetail
{
    [JsonProperty("code")]
    public string Code { get; set; } = string.Empty;

    [JsonProperty("message")]
    public string Message { get; set; } = string.Empty;
}
