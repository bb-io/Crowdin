using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers
{
    public class FilePriorityHandler : IStaticDataSourceItemHandler
    {
        public IEnumerable<DataSourceItem> GetData()
        {
            return new List<DataSourceItem>
            {
                new DataSourceItem("low", "Low"),
                new DataSourceItem("normal", "Normal"),
                new DataSourceItem("high", "High")
            };
        }
    }
}
