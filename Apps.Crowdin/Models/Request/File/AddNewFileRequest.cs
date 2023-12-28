using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Crowdin.Models.Request.File;

public class AddNewFileRequest
{    
    public FileReference File { get; set; }
    
    [Display("Branch ID")]
    public string? BranchId { get; set; }

    [Display("Directory ID")]
    public string? DirectoryId { get; set; }

    public string? Title { get; set; }

    [Display("Excluded target languages")]
    public IEnumerable<string>? ExcludedTargetLanguages { get; set; }

    [Display("Attach label IDs")]
    public IEnumerable<int>? AttachLabelIds { get; set; }
}