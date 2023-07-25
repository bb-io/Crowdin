using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.Translation;

public class ListLanguageTranslationsRequest
{
    [Display("Project ID")]
    public string ProjectId { get; set; }
    
    [Display("Language ID")]
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