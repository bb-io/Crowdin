using Apps.Crowdin.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Crowdin.Api.MachineTranslationEngines;

namespace Apps.Crowdin.Models.Entities;

public class MtStringsTranslationEntity
{
    [Display("Source language")]
    [DataSource(typeof(LanguagesDataHandler))]
    public string SourceLanguageId { get; set; }

    [Display("Target language")]
    [DataSource(typeof(LanguagesDataHandler))]
    public string TargetLanguageId { get; set; }

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