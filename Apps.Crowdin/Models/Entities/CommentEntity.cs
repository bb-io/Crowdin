using Blackbird.Applications.Sdk.Common;
using Crowdin.Api.StringComments;

namespace Apps.Crowdin.Models.Entities;

public class CommentEntity
{
    [Display("ID")]
    public string Id { get; set; }

    [Display("User ID")]
    public string UserId { get; set; }

    [Display("String ID")]
    public string StringId { get; set; }

    public string Text { get; set; }

    [Display("Language ID")]
    public string LanguageId { get; set; }

    public string Type { get; set; }
    
    [Display("Issue type")]
    public string? IssueType { get; set; }
    
    [Display("Issue status")]
    public string IssueStatus { get; set; }
    
    [Display("Resolved at")]
    public DateTime ResolvedAt { get; set; }
    
    [Display("Created at")]
    public DateTime CreatedAt { get; set; }

    public CommentEntity(StringComment stringComment)
    {
        Id = stringComment.Id.ToString();
        UserId = stringComment.UserId.ToString();
        StringId = stringComment.StringId.ToString();
        Text = stringComment.Text;
        LanguageId = stringComment.LanguageId;
        Type = stringComment.Type.ToString();
        IssueType = stringComment.IssueType?.ToString();
        IssueStatus = stringComment.IssueStatus.ToString();
        ResolvedAt = stringComment.ResolvedAt.DateTime;
        CreatedAt = stringComment.CreatedAt.DateTime;
    }
}