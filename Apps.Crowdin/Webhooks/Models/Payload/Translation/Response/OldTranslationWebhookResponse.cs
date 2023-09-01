using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Webhooks.Models.Payload.Translation.Response;

public class OldTranslationWebhookResponse
{
    [Display("Translation ID")]
    public string Id { get; set; }

    [Display("Text")]
    public string Text { get; set; }

    [Display("Plural category name")]
    public string PluralCategoryName { get; set; }

    public int? Rating { get; set; }
    
    public string Provider { get; set; }
    
    [Display("Is pre-translated")]
    public bool IsPretranslated { get; set; }

    [Display("Created at")]
    public DateTime? CreatedAt { get; set; }

    public OldTranslationWebhookResponse(OldTranslationPayload payload)
    {
        Id = payload.Id;
        Text = payload.Text;
        PluralCategoryName = payload.PluralCategoryName;
        Rating = payload.Rating;
        Provider = payload.Provider;
        IsPretranslated = payload.IsPretranslated;
        CreatedAt = payload.CreatedAt;
    }
}