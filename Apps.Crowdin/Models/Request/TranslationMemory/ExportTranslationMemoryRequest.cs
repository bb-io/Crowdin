using Apps.Crowdin.DataSourceHandlers;
using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.TranslationMemory;

public class ExportTranslationMemoryRequest
{
    [Display("Source language")]
    [DataSource(typeof(LanguagesDataHandler))]
    public string? SourceLanguageId { get; set; }

    [Display("Target language")]
    [DataSource(typeof(LanguagesDataHandler))]
    public string? TargetLanguageId { get; set; }

    [StaticDataSource(typeof(TmFileFormatHandler))]
    public string? Format { get; set; }
}