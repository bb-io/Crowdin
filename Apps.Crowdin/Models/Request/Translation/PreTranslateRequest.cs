using Apps.Crowdin.DataSourceHandlers;
using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.Translation;

public class PreTranslateRequest
{
    [DataSource(typeof(LanguagesDataHandler))]
    [Display("Language IDs")] public IEnumerable<string> LanguageIds { get; set; }

    [Display("File IDs")] public IEnumerable<string> FileIds { get; set; }

    [DataSource(typeof(MtEnginesDataHandler))]
    [Display("Engine ID")] public string? EngineId { get; set; }

    [Display("Duplicate translations")] public bool? DuplicateTranslations { get; set; }

    [Display("Translate untranslated only")]
    public bool? TranslateUntranslatedOnly { get; set; }

    [Display("Translate with perfect match only")]
    public bool? TranslateWithPerfectMatchOnly { get; set; }
    
    [StaticDataSource(typeof(AutoApproveOptionHandler))]    
    [Display("Auto approve option")]
    public string? AutoApproveOption { get; set; }

    [StaticDataSource(typeof(PreTranslationMethodHandler))]
    [Display("Pre translation method")]
    public string? Method { get; set; }

}