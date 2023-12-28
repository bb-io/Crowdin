using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Crowdin.Models.Request.Storage;

public class AddStorageRequest
{
    public FileReference File { get; set; }
    [Display("File name")] public string? FileName { get; set; }
}