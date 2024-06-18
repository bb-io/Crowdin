using Crowdin.Api.SourceFiles;
using Newtonsoft.Json;

namespace Apps.Crowdin.Models.Dtos;

public class AddFileRequestDto
{
    [JsonProperty("storageId")]
    public long StorageId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("branchId")]
    public int? BranchId { get; set; }

    [JsonProperty("directoryId")]
    public int? DirectoryId { get; set; }

    [JsonProperty("title")]
    public string? Title { get; set; }

    [JsonProperty("context")]
    public string? Context { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("importOptions")]
    public FileImportOptions? ImportOptions { get; set; }

    [JsonProperty("exportOptions")]
    public FileExportOptions? ExportOptions { get; set; }

    [JsonProperty("excludedTargetLanguages")]
    public List<string>? ExcludedTargetLanguages { get; set; }

    [JsonProperty("attachLabelIds")]
    public ICollection<int>? AttachLabelIds { get; set; }
}