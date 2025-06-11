using Apps.Crowdin.DataSourceHandlers;
using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.Glossary;

public class GetGlossaryRequest
{
    [Display("Glossary ID"), DataSource(typeof(GlossaryDataHandler))]
    public string GlossaryId { get; set; } = default!;

    [Display("File format", Description = "TBX is the default and recommended format as it is interoperable with other apps and actions"),
        StaticDataSource(typeof(GlossaryFileFormatHandler))]
    public string? Format { get; set; }
}