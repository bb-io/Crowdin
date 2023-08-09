using Apps.Crowdin.DataSourceHandlers;
using Apps.Crowdin.Models.Request.Project;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.Translation;

public class ListStringTranslationsRequest : ProjectRequest
{
    [Display("Language")]
    [DataSource(typeof(LanguagesDataHandler))]
    public string LanguageId { get; set; }
    
    [Display("String ID")]
    public string StringId { get; set; }
    
    [Display("Denormalize placeholders")]
    public bool? DenormalizePlaceholders { get; set; }
}