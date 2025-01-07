using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.Directory;

public class ListDirectoriesRequest
{
    [Display("Branch ID")]
    public string? BranchId { get; set; }
    
    [Display("Directory ID")]
    public string? DirectoryId { get; set; }
    
    public string? Name { get; set; }
}