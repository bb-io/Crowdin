using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.SourceString;

public class ListStringsRequest
{
    [Display("Project ID")] public string ProjectId { get; set; }

    [Display("Denormalize placeholders")] public bool? DenormalizePlaceholders { get; set; }

    [Display("Label IDs")] public string? LabelIds { get; set; }

    [Display("File ID")] public string? FileId { get; set; }

    [Display("Branch ID")] public string? BranchId { get; set; }

    [Display("Directory ID")] public string? DirectoryId { get; set; }

    public string? CroQl { get; set; }
    public string? Filter { get; set; }
    public string? Scope { get; set; }
}