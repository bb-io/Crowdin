using Apps.Crowdin.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.TranslationMemory;

public class TranslationMemoryRequest
{
    [Display("Translation memory")]
    [DataSource(typeof(TmDataHandler))]
    public string TranslationMemoryId { get; set; }
}