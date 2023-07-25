using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.Comments;

public class ListCommentsRequest
{
    [Display("Project ID")]
    public string ProjectId { get; set; }
    
    [Display("String ID")]
    public string? StringId { get; set; }
    
    public string? Type { get; set; }
    
    [Display("Issue types")]
    public IEnumerable<string>? IssueTypes { get; set; }
    
    [Display("Issue status")]
    public string? IssueStatus { get; set; }
}