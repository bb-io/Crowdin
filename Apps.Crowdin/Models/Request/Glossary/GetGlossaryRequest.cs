using Apps.Crowdin.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.Glossary;

public class GetGlossaryRequest
{
    [Display("Glossary ID"), DataSource(typeof(GlossaryDataHandler))]
    public string GlossaryId { get; set; } = default!;
}