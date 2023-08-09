using Apps.Crowdin.DataSourceHandlers;
using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Apps.Crowdin.Models.Request.Project;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.Comments;

public class AddNewCommentRequest : ProjectRequest
{
    [Display("String ID")] public string StringId { get; set; }

    [Display("Target language")] 
    [DataSource(typeof(LanguagesDataHandler))]
    public string TargetLanguageId { get; set; }

    public string Text { get; set; }
    
    [DataSource(typeof(StringCommentTypeHandler))]
    public string Type { get; set; }

    [Display("Issue type")] 
    [DataSource(typeof(IssueTypeHandler))]
    public string? IssueType { get; set; }
}