using Crowdin.Api;
using Crowdin.Api.SourceFiles;
using Newtonsoft.Json;

namespace Apps.Crowdin.Models.Dtos;

public class FileDto
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("projectId")]
    public int ProjectId { get; set; }

    [JsonProperty("branchId")]
    public int? BranchId { get; set; }

    [JsonProperty("directoryId")]
    public int? DirectoryId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("title")]
    public string? Title { get; set; }

    [JsonProperty("context")]
    public string? Context { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("path")]
    public string Path { get; set; }

    [JsonProperty("status")]
    public FileStatus Status { get; set; }

    [JsonProperty("revisionId")]
    public int RevisionId { get; set; }

    [JsonProperty("priority")]
    public Priority Priority { get; set; }

    [JsonProperty("importOptions")]
    public FileImportOptions? ImportOptions { get; set; }

    [JsonProperty("exportOptions")]
    public FileExportOptions? ExportOptions { get; set; }

    [JsonProperty("excludedTargetLanguages")]
    public string[]? ExcludedTargetLanguages { get; set; }

    [JsonProperty("createdAt")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonProperty("updatedAt")]
    public DateTimeOffset? UpdatedAt { get; set; }
}