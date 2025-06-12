using Apps.Crowdin.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.Translation;

public class DownloadFileTranslationRequest
{
    [Display("File ID")]
    [DataSource(typeof(FileDataHandler))]
    public string FileId { get; set; }

    [Display("Target language")]
    [DataSource(typeof(ProjectTargetLanguageDataHandler))]
    public string TargetLanguage { get; set; }

    [Display("Skip untranslated strings?")]
    public bool? SkipUntranslatedStrings { get; set; }

    [Display("Skip untranslated files?")]
    public bool? SkipUntranslatedFiles { get; set; }

    [Display("Export approved only?")]
    public bool? ExportApprovedOnly { get; set; }
}