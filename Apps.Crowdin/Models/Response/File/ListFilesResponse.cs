using Apps.Crowdin.Models.Entities;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Response.File;

public class ListFilesResponse(FileEntity[] files) 
{
    public FileEntity[] Files { get; set; } = files;

    [Display("File IDs")]
    public IEnumerable<string> FileIds { get; set; } = files.Select(x => x.Id).ToList();
}