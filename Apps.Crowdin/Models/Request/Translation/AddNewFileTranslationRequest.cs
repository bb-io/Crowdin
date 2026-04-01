using Apps.Crowdin.DataSourceHandlers;
using Apps.Crowdin.Models.Request.Project;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Crowdin.Models.Request.Translation;

public class AddNewFileTranslationRequest : ProjectRequest
{
    [Display("Language ID"), DataSource(typeof(LanguagesDataHandler))]
    public string LanguageId { get; set; } = string.Empty;

    [Display("File")]
    public FileReference File { get; set; } = null!;

    [Display("Source file ID")]
    public string? SourceFileId { get; set; }

    [Display("Import equal suggestions")]
    public bool? ImportEqSuggestions { get; set; }

    [Display("Auto approve imported")]
    public bool? AutoApproveImported { get; set; }

    [Display("Translate hidden")]
    public bool? TranslateHidden { get; set; }

    public AddNewFileTranslationRequest Validate()
    {
        if (string.IsNullOrEmpty(LanguageId))
        {
            throw new PluginMisconfigurationException(
                "Language ID cannot be null or empty. Please provide a valid language ID.");
        }

        if (string.IsNullOrEmpty(ProjectId))
        {
            throw new PluginMisconfigurationException(
                "Project ID cannot be null or empty. Please provide a valid project ID.");
        }

        return this;
    }
}