using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers;

public class PluralCategoryNameHandler : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData() => new()
    {
        { "Zero", "Zero" },
        { "One", "One" },
        { "Two", "Two" },
        { "Few", "Few" },
        { "Many", "Many" },
        { "Other", "Other" },
    };
}