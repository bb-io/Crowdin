using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers;

public class PreTranslationStatusDataSource : IStaticDataSourceItemHandler
{
    private static Dictionary<string, string> Data => new()
    {
        { "created", "Created" },
        { "in_progress", "In progress" },
        { "canceled", "Canceled" },
        { "failed", "Failed" },
        { "finished", "Finished" },
    };
            
    public IEnumerable<DataSourceItem> GetData()
    {
        return Data.Select(x => new DataSourceItem(x.Key, x.Value));
    }
}