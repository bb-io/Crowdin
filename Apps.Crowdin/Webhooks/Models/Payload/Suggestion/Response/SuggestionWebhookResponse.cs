using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Webhooks.Models.Payload.String;
using Apps.Crowdin.Webhooks.Models.Payload.Suggestion.Wrappers;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Webhooks.Models.Payload.Suggestion.Response;

public class SuggestionWebhookResponse : CrowdinWebhookResponse<SuggestionWrapper>
{
    [Display("ID")]
    public string Id { get; set; }
    public string Text { get; set; }
    public string Rating { get; set; }
    
    [Display("Created at")]
    public DateTime CreatedAt { get; set; }
    
    [Display("Target language ID")]
    public string TargetLanguageId { get; set; }

    public UserEntity User { get; set; }
    public StringWebhookResponseEntity String { get; set; }
    
    public override void ConfigureResponse(SuggestionWrapper wrapper)
    {
        Id = wrapper.Translation.Id;
        Text = wrapper.Translation.Text;
        Rating = wrapper.Translation.Rating;
        CreatedAt = wrapper.Translation.CreatedAt;
        TargetLanguageId = wrapper.Translation.TargetLanguage.Id;
        User = new(wrapper.Translation.User);
        String = new(wrapper.Translation.String);
    }
}