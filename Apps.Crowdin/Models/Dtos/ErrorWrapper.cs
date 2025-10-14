using Newtonsoft.Json;

namespace Apps.Crowdin.Models.Dtos;
public class ErrorWrapper
{
    [JsonProperty("error")]
    public ErrorDto Error { get; set; } = new();
}
