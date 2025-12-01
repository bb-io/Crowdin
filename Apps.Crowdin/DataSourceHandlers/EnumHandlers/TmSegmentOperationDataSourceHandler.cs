using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers
{
    public class TmSegmentOperationDataSourceHandler : IStaticDataSourceItemHandler
    {
        public IEnumerable<DataSourceItem> GetData()
        {
            return new[]
            {
                new DataSourceItem("add", "Add record"),
                new DataSourceItem("replace", "Replace text"),
                new DataSourceItem("remove", "Remove record"),
            };
        }
    }
}
