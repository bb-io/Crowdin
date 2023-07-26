using Apps.Crowdin.Webhooks.Models.Payload.String;
using Crowdin.Api.StringTranslations;

namespace Apps.Crowdin.Webhooks.Models.Payload.StringComment;

public class CommentWebhookPayload
{
    public string Id { get; set; }
    public string Text { get; set; }
    public string Type { get; set; }
    public string IssueType { get; set; }
    public string IssueStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public StringPayload String { get; set; }
    public LanguagePayload TargetLanguage { get; set; }
    public User User { get; set; }
    public User CommentResolver { get; set; }
}