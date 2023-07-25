using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Entities;

public class MtTextTranslationEntity
{
    [Display("Source language ID")] public string SourceLanguageId { get; set; }
    [Display("Target language ID")] public string TargetLanguageId { get; set; }
    [Display("Source text")] public string SourceText { get; set; }
    [Display("Translation")] public string Translation { get; set; }


    public MtTextTranslationEntity(MtStringsTranslationEntity translation)
    {
        SourceLanguageId = translation.SourceLanguageId;
        TargetLanguageId = translation.TargetLanguageId;
        SourceText = translation.SourceStrings.First();
        Translation = translation.Translations.First();
    }
}