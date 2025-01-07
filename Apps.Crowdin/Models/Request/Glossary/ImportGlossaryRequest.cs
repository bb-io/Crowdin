using Apps.Crowdin.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Crowdin.Models.Request.Glossary;

public class ImportGlossaryRequest
{
    [Display("Glossary name")]
    public string? GlossaryName { get; set; }

    [Display("Language"), DataSource(typeof(LanguagesDataHandler))]
    public string? LanguageCode { get; set; }
    
    [Display("Group"), DataSource(typeof(ProjectGroupDataHandler))]
    public string? GroupId { get; set; }
    
    public FileReference File { get; set; }
}