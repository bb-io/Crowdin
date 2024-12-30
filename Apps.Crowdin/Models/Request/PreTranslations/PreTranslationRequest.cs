using Apps.Crowdin.DataSourceHandlers;
using Apps.Crowdin.Models.Request.Project;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.PreTranslations;

public class PreTranslationRequest : ProjectRequest
{
    [Display("Pre-translation ID"), DataSource(typeof(PreTranslationDataSource))]
    public string PreTranslationId { get; set; } = string.Empty;
}