using Newtonsoft.Json;

namespace Apps.Crowdin.Models.Dtos;

public class DataWrapper<T>
{
    [JsonProperty("data")]
    public T Data { get; set; }
}