using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.TranslationMemory;

public class AddTranslationMemorySegmentRequest
{
    [Display("Translation memory ID")]
    public string TranslationMemoryId { get; set; }    
    
    [Display("Language ID")]
    public string LanguageId { get; set; }
    public string Text { get; set; }
}