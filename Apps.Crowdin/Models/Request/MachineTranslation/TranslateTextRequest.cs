using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.MachineTranslation;

public class TranslateTextRequest
{
    [Display("Source language ID")] public string SourceLanguageId { get; set; }
    [Display("Target language ID")] public string TargetLanguageId { get; set; }
    public string Text { get; set; }
    [Display("Language recognition provider")] public string LanguageRecognitionProvider { get; set; }
}