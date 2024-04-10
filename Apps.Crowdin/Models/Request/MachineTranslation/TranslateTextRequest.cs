using Apps.Crowdin.DataSourceHandlers;
using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.MachineTranslation;

public class TranslateTextRequest
{
    [Display("Source language")]
    [DataSource(typeof(LanguagesDataHandler))]
    public string SourceLanguageId { get; set; }
    
    [Display("Target language")]
    [DataSource(typeof(LanguagesDataHandler))]
    public string TargetLanguageId { get; set; }
    
    public string Text { get; set; }
    
    [Display("Language recognition provider")]
    [StaticDataSource(typeof(LanguageRecognitionProviderHandler))]
    public string LanguageRecognitionProvider { get; set; }
}