using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.Translation;

public class PreTranslateRequest
{
    [Display("Language IDs")] public IEnumerable<string> LanguageIds { get; set; }

    [Display("File IDs")] public IEnumerable<string> FileIds { get; set; }

    [Display("Engine ID")] public string? EngineId { get; set; }

    [Display("Duplicate translations")] public bool? DuplicateTranslations { get; set; }

    [Display("Translate untranslated only")]
    public bool? TranslateUntranslatedOnly { get; set; }

    [Display("Translate with perfect match only")]
    public bool? TranslateWithPerfectMatchOnly { get; set; }

    [Display("Mark added translations as done")]
    public bool? MarkAddedTranslationsAsDone { get; set; }
}