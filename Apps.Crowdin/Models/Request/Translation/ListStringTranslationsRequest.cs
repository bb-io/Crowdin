using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.Translation;

public class ListStringTranslationsRequest
{
    [Display("Project ID")]
    public string ProjectId { get; set; }
    
    [Display("Language ID")]
    public string LanguageId { get; set; }
    
    [Display("String ID")]
    public string StringId { get; set; }
    
    [Display("Denormalize placeholders")]
    public bool? DenormalizePlaceholders { get; set; }
}