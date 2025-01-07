using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.Directory;

public class AddNewDirectoryRequest
{
    public string Path { get; set; }
    public string? Title { get; set; }
    
    [Display("Path contains file")]
    public bool? PathContainsFile { get; set; }
    [Display("Branch ID")] public string? BranchId { get; set; }
    [Display("Directory ID")] public string? DirectoryId { get; set; }
}