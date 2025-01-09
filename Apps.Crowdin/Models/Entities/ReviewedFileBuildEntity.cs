using Blackbird.Applications.Sdk.Common;
using Crowdin.Api.SourceFiles;

namespace Apps.Crowdin.Models.Entities;

public class ReviewedFileBuildEntity(ReviewedStringBuild build)
{
    [Display("ID")]
    public string Id { get; set; } = build.Id.ToString();

    [Display("Project ID")]
    public string ProjectId { get; set; } = build.ProjectId.ToString();

    public string Status { get; set; } = build.Status.ToString();

    public int Progress { get; set; } = build.Progress;

    [Display("Branch ID")]
    public string BranchId { get; set; } = build.Attributes.BranchId.ToString();

    [Display("Target language")]
    public string TargetLanguage { get; set; } = build.Attributes.TargetLanguageId;
}