using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.Project;

public class BuildProjectRequest
{
    [Display("Branch ID")]
    public string? BranchId { get; set; }

    [Display("Target language IDs")]
    public IEnumerable<string>? TargetLanguageIds { get; set; }

    [Display("Skip untranslated strings")]
    public bool? SkipUntranslatedStrings { get; set; }

    [Display("Skip untranslated files")]
    public bool? SkipUntranslatedFiles { get; set; }

    [Display("Export with min approvals count")]
    public int? ExportWithMinApprovalsCount { get; set; }
}