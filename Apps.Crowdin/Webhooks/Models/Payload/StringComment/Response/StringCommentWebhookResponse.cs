using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Webhooks.Models.Payload.String;
using Apps.Crowdin.Webhooks.Models.Payload.StringComment.Wrappers;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Webhooks.Models.Payload.StringComment.Response;

public class StringCommentWebhookResponse : CrowdinWebhookResponse<StringCommentWrapper>
{
    [Display("ID")] public string Id { get; set; }
    public string Text { get; set; }
    public string Type { get; set; }

    [Display("Issue type")] public string IssueType { get; set; }

    [Display("Issue status")] public string IssueStatus { get; set; }

    [Display("Created at")] public DateTime? CreatedAt { get; set; }
    [Display("Target language ID")] public string TargetLanguageId { get; set; }
    public StringWebhookResponseEntity String { get; set; }
    public UserEntity User { get; set; }
    [Display("Comment resolver")] public UserEntity CommentResolver { get; set; }

    public override void ConfigureResponse(StringCommentWrapper wrapper)
    {
        Id = wrapper.Comment.Id;
        Text = wrapper.Comment.Text;
        Type = wrapper.Comment.Type;
        IssueType = wrapper.Comment.IssueType;
        IssueStatus = wrapper.Comment.IssueStatus;
        CreatedAt = wrapper.Comment.CreatedAt;
        String = new(wrapper.Comment.String);
        TargetLanguageId = wrapper.Comment.TargetLanguage.Id;
        User = new(wrapper.Comment.User);
        CommentResolver = new(wrapper.Comment.CommentResolver);
    }
}