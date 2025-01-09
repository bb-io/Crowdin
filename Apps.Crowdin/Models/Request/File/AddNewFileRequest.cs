using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.File;

public class AddNewFileRequest : ManageFileRequest
{    
    [Display("File name")]
    public string? Name { get; set; }
    
    [Display("Branch ID")]
    public string? BranchId { get; set; }

    [Display("Directory ID")]
    public string? DirectoryId { get; set; }

    [Display("File context")]
    public string? Context { get; set; }

    public string? Title { get; set; }

    [Display("Excluded target languages")]
    public IEnumerable<string>? ExcludedTargetLanguages { get; set; }

    [Display("Attach label IDs")]
    public IEnumerable<int>? AttachLabelIds { get; set; }
}