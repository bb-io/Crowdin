using Apps.Crowdin.Webhooks.Models.Payload.String;
using Crowdin.Api.StringTranslations;

namespace Apps.Crowdin.Webhooks.Models.Payload.Suggestion;

public class SuggestionWebhookPayload
{
    public string Id { get; set; }
    public string Text { get; set; }
    public string Rating { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public User User { get; set; }
    public LanguagePayload TargetLanguage { get; set; }
    public StringPayload String { get; set; }
}