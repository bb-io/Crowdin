namespace Apps.Crowdin.Webhooks.Models.Payload.Translation;

public class OldTranslationPayload
{
    public string Id { get; set; }

    public string Text { get; set; }

    public string PluralCategoryName { get; set; }

    public int? Rating { get; set; }
    
    public string Provider { get; set; }
    
    public bool IsPretranslated { get; set; }

    public DateTime? CreatedAt { get; set; }
}