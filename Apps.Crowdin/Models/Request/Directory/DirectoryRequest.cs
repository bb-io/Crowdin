using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.Directory;

public class DirectoryRequest
{
    [Display("Directory ID")]
    public string DirectoryId { get; set; }
}