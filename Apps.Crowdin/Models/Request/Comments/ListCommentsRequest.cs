using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Apps.Crowdin.Models.Request.Project;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Crowdin.Models.Request.Comments;

public class ListCommentsRequest : ProjectRequest
{
    [Display("String ID")]
    public string? StringId { get; set; }
    
    [StaticDataSource(typeof(StringCommentTypeHandler))]
    public string? Type { get; set; }
    
    [Display("Issue types")]
    public IEnumerable<string>? IssueTypes { get; set; }
    
    [Display("Issue status")]
    [StaticDataSource(typeof(IssueStatusHandler))]
    public string? IssueStatus { get; set; }
}