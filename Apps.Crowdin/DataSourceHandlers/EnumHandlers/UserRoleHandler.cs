using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers;

public class UserRoleHandler : IStaticDataSourceItemHandler
{
    private static Dictionary<string, string> Data => new()
    {
        {"all", "All"},
        {"manager", "Manager"},
        {"developer", "Developer"},
        {"language_coordinator", "Language Coordinator"},
        {"proofreader", "Proofreader"},
        {"translator", "Translator"},
        {"blocked", "Blocked"},
        {"pending", "Pending"}
    };

    public IEnumerable<DataSourceItem> GetData()
    {
        return Data.Select(x => new DataSourceItem(x.Key, x.Value));
    }
}