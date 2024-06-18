using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
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

    [Display("Type", Description = "File type, by default: auto"), StaticDataSource(typeof(TypeDataSource))]
    public string? Type { get; set; }
}