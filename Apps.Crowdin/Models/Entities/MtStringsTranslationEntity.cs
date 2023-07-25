using Blackbird.Applications.Sdk.Common;
using Crowdin.Api.MachineTranslationEngines;

namespace Apps.Crowdin.Models.Entities;

public class MtStringsTranslationEntity
{
    [Display("Source language ID")] public string SourceLanguageId { get; set; }
    [Display("Target language ID")] public string TargetLanguageId { get; set; }
    [Display("Source strings")] public string[] SourceStrings { get; set; }
    [Display("Translated strings")] public string[] Translations { get; set; }


    public MtStringsTranslationEntity(MtTranslation translation)
    {
        SourceLanguageId = translation.SourceLanguageId;
        TargetLanguageId = translation.TargetLanguageId;
        SourceStrings = translation.Strings;
        Translations = translation.Translations;
    }
}