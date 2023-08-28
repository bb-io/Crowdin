using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.Storage;

public class AddStorageRequest
{
    public Blackbird.Applications.Sdk.Common.Files.File File { get; set; }
    [Display("File name")] public string? FileName { get; set; }
}