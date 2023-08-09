using Apps.Crowdin.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.TranslationMemory;

public class AddTranslationMemoryRequest
{
    public string Name { get; set; }
    
    [Display("Language")] 
    [DataSource(typeof(LanguagesDataHandler))]
    public string LanguageId { get; set; }
    
    [Display("Group ID")] public string? GroupId { get; set; }
}