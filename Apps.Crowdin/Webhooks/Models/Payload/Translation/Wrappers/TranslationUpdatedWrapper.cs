using Crowdin.Api.StringTranslations;

namespace Apps.Crowdin.Webhooks.Models.Payload.Translation.Wrappers;

public class TranslationUpdatedWrapper
{
    public StringTranslation OldTranslation { get; set; }
    public StringTranslation NewTranslation { get; set; }
}