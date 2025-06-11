using Apps.Crowdin.DataSourceHandlers;
using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Apps.Crowdin.Models.Request.Project;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.SourceString;

public class ListStringsRequest : ProjectRequest
{
    [Display("Denormalize placeholders")] public bool? DenormalizePlaceholders { get; set; }

    [Display("Label IDs")] public string? LabelIds { get; set; }

    [Display("File ID")]
    [DataSource(typeof(FileDataHandler))]
    public string? FileId { get; set; }

    [Display("Branch ID")]
    [DataSource(typeof(BranchDataHandler))]
    public string? BranchId { get; set; }

    [Display("Directory ID")]
    [DataSource(typeof(DirectoryDataHandler))]
    public string? DirectoryId { get; set; }

    public string? CroQl { get; set; }
    public string? Filter { get; set; }
    
    [StaticDataSource(typeof(StringScopeHandler))]
    public string? Scope { get; set; }
}