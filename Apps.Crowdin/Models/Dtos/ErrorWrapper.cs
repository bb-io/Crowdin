using Newtonsoft.Json;

namespace Apps.Crowdin.Models.Dtos;

public class ErrorWrapper
{
    [JsonProperty("error")]
    public ErrorsDto Error { get; set; } = new();
}
