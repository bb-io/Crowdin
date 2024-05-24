using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers
{
    public class AutoApproveOptionHandler : IStaticDataSourceHandler
    {
            public Dictionary<string, string> GetData() => new()
        {
            { "All", "All" },
            { "None", "None" },
            { "PerfectMatchOnly", "Perfect match only" },
            { "ExceptAutoSubstituted", "Except auto substituted" }
        };
    }
}
