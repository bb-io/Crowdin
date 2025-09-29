using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers;

public class VendorStatusDataHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData()
    {
        return new List<DataSourceItem>
        {
            new DataSourceItem("pending", "Pending"),
            new DataSourceItem("confirmed", "Confirmed"),
            new DataSourceItem("rejected", "Rejected"),
            new DataSourceItem("deleted", "Deleted"),
        };
    }
}
