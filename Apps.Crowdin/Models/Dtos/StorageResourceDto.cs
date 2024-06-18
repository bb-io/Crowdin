using Newtonsoft.Json;

namespace Apps.Crowdin.Models.Dtos;

public class StorageResourceDto
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("fileName")]
    public string FileName { get; set; } = string.Empty;
}