using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.TranslationMemory;

public class DownloadTranslationMemoryRequest
{
    [Display("Translation memory ID")]
    public string TrasnlationMemoryId { get; set; }
    
    [Display("Export ID")]
    public string ExportId { get; set; }
}