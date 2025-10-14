using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers;

public class VendorTaskStatusTypeHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData()
    {
        return new List<DataSourceItem>
        {
            new DataSourceItem("todo", "To do"),
            new DataSourceItem("in_progress", "In progress")
        };
    }
}