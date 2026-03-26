using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Exceptions;

namespace Apps.Crowdin.Models.Request.Filter;

public class FieldsFilterRequest
{
    [Display("Field names", Description = "Must match the length and order of the 'Field value contains' input")]
    public List<string>? FieldNamesFilter { get; set; }

    [Display("Field value contains")]
    public List<string>? FieldValuesFilter { get; set; }

    public FieldsFilterRequest Validate()
    {
        if (FieldNamesFilter?.Count != FieldValuesFilter?.Count)
        {
            throw new PluginMisconfigurationException(
                "'Field names' input must match the length and order of the 'Field value contains' input");
        }

        return this;
    }
}
