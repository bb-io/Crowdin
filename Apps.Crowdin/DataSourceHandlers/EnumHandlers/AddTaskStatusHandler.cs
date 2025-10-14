using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers;

public class AddTaskStatusHandler : IStaticDataSourceItemHandler
{    
    public IEnumerable<DataSourceItem> GetData()
    {
        return new List<DataSourceItem>
        {
            new DataSourceItem("Todo", "To do"),
            new DataSourceItem("InProgress", "In progress")
        };
    }
}