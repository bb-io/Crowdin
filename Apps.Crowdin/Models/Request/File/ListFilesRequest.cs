using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Crowdin.Models.Request.File;

public class ListFilesRequest
{
    [Display("Branch ID")]
    public string? BranchId { get; set; }
    [Display("Directory ID")]
    public string? DirectoryId { get; set; }

    public string? Filter { get; set; }

    [Display("Recursive", Description = "Search through all subdirectories")]
    public bool? Recursive { get; set; }

    [Display("Status")]
    [StaticDataSource(typeof(FileStatusHandler))]
    public string? Status { get; set; }

    [Display("Priority")]
    [StaticDataSource(typeof(FilePriorityHandler))]
    public string? Priority { get; set; }

    [Display("Created after")]
    public DateTime? CreatedAfter { get; set; }

    [Display("Created before")]
    public DateTime? CreatedBefore { get; set; }
    [Display("Updated after")]
    public DateTime? UpdatedAfter { get; set; }
    [Display("Updated before")]
    public DateTime? UpdatedBefore { get; set; }
}