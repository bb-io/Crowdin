using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.Comments;

public class AddNewCommentRequest
{
    [Display("Project ID")] public string ProjectId { get; set; }

    [Display("String ID")] public string StringId { get; set; }

    [Display("Target language ID")] public string TargetLanguageId { get; set; }

    public string Text { get; set; }
    public string Type { get; set; }

    [Display("Issue type")] public string? IssueType { get; set; }
}