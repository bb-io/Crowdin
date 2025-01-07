using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers;

public class TaskStatusHandler : IStaticDataSourceItemHandler
{
    private static Dictionary<string, string> Data => new()
    {
        { "Todo", "To do" },
        { "InProgress", "In progress" },
        { "Done", "Done" },
        { "Closed", "Closed" },
    };
    
    public IEnumerable<DataSourceItem> GetData()
    {
        return Data.Select(x => new DataSourceItem(x.Key, x.Value));
    }
}