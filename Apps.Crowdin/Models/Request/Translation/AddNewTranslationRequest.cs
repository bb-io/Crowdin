using Apps.Crowdin.DataSourceHandlers;
using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Apps.Crowdin.Models.Request.Project;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.Translation;

public class AddNewTranslationRequest : ProjectRequest
{
    [Display("Language")] 
    [DataSource(typeof(LanguagesDataHandler))]
    public string LanguageId { get; set; }
    
    [Display("String ID")] public string StringId { get; set; }
    [Display("Text")] public string Text { get; set; }
    
    [Display("Plural category")]
    [StaticDataSource(typeof(PluralCategoryNameHandler))]
    public string? PluralCategoryName { get; set; }
}