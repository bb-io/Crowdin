using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.File;

public class ListFilesRequest
{
    [Display("Branch ID")] public string? BranchId { get; set; }
    [Display("Directory ID")] public string? DirectoryId { get; set; }
    public string? Filter { get; set; }
}