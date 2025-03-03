using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers;

public class PendingTaskTypeHandler : IStaticDataSourceItemHandler
{
    private static Dictionary<string, string> Data => new()
    {
       { "1", "Proofread" },
    };
    
    public IEnumerable<DataSourceItem> GetData()
    {
        return Data.Select(x => new DataSourceItem(x.Key, x.Value));
    }
}