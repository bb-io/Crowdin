using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.MachineTranslation;

public class TranslateStringsRequest
{
    [Display("Target language ID")] public string TargetLanguageId { get; set; }
    public IEnumerable<string> Text { get; set; }
    [Display("Source language ID")] public string? SourceLanguageId { get; set; }
    [Display("Language recognition provider")] public string? LanguageRecognitionProvider { get; set; }

    
    public TranslateStringsRequest()
    {
    }

    public TranslateStringsRequest(TranslateTextRequest input)
    {
        TargetLanguageId = input.TargetLanguageId;
        SourceLanguageId = input.SourceLanguageId;
        LanguageRecognitionProvider = input.LanguageRecognitionProvider;
        Text = new[] { input.Text };
    }
}