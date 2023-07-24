using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.TranslationMemory;

public class ExportTranslationMemoryRequest
{
    [Display("Source language ID")]
    public string? SourceLanguageId { get; set; }

    [Display("Target language ID")]
    public string? TargetLanguageId { get; set; }

    public string? Format { get; set; }
}