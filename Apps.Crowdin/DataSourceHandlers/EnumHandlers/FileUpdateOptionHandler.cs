using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers;

public class FileUpdateOptionHandler : IStaticDataSourceItemHandler
{
    private static Dictionary<string, string> Data => new()
    {
        { "clear_translations_and_approvals", "Clear translations and approvals" },
        { "keep_translations", "Keep translations" },
        { "keep_translations_and_approvals", "Keep translations and approvals" },
    };
    
    public IEnumerable<DataSourceItem> GetData()
    {
        return Data.Select(x => new DataSourceItem(x.Key, x.Value));
    }
}