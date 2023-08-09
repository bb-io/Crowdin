using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.TranslationMemory;

public class DownloadTranslationMemoryRequest : TranslationMemoryRequest
{
    [Display("Export ID")]
    public string ExportId { get; set; }
}