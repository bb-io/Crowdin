namespace Apps.Crowdin.Webhooks.Models.Payload.Translation.Wrappers;

public class TranslationUpdatedWrapper
{
    public OldTranslationPayload OldTranslation { get; set; }
    public NewTranslationPayload NewTranslation { get; set; }
}