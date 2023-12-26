using Apps.Crowdin.DataSourceHandlers;
using Apps.Crowdin.Models.Request.Project;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Crowdin.Models.Request.Translation;

public class AddNewFileTranslationRequest : ProjectRequest
{
    [Display("Language")]
    [DataSource(typeof(LanguagesDataHandler))]
    public string LanguageId { get; set; }

    [Display("File")]
    public FileReference File { get; set; }

    [Display("Source file ID")]
    public string SourceFileId { get; set; }

    [Display("Import equal suggestions")]
    public bool? ImportEqSuggestions { get; set; }

    [Display("Auto approve imported")]
    public bool? AutoApproveImported { get; set; }

    [Display("Translate hidden")]
    public bool? TranslateHidden { get; set; }
}