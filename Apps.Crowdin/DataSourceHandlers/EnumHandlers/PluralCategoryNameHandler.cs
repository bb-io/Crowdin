using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers;

public class PluralCategoryNameHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        { "Zero", "Zero" },
        { "One", "One" },
        { "Two", "Two" },
        { "Few", "Few" },
        { "Many", "Many" },
        { "Other", "Other" },
    };
}