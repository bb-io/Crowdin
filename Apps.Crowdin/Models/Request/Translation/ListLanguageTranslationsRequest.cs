using Apps.Crowdin.DataSourceHandlers;
using Apps.Crowdin.Models.Request.Project;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.Translation;

public class ListLanguageTranslationsRequest : ProjectRequest
{
    [Display("Language")]
    [DataSource(typeof(LanguagesDataHandler))]
    public string LanguageId { get; set; }
    
    [Display("String IDs")]
    public string? StringIds { get; set; }
    
    [Display("Label IDs")]
    public string? LabelIds { get; set; }

    [Display("File ID")]
    public string? FileId { get; set; }

    public string? CroQL { get; set; }

    [Display("Denormalize placeholders")]
    public bool? DenormalizePlaceholders { get; set; }
}