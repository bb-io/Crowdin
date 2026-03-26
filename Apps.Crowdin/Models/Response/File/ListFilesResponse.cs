using Apps.Crowdin.Models.Entities;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Response.File;

public record ListFilesResponse(List<FileEntity> Files) 
{
    public List<FileEntity> Files { get; set; } = Files;

    [Display("File IDs")]
    public IEnumerable<string> FileIds { get; set; } = Files.Select(x => x.Id).ToList();
}