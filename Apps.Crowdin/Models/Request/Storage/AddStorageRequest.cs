using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.Storage;

public class AddStorageRequest
{
    public byte[] File { get; set; }
    [Display("File name")] public string FileName { get; set; }
}