using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.File;

public class FileRequest
{
    [Display("File ID")]
    public string FileId { get; set; }
}