using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers
{
    public class TmSegmentOperationDataSourceHandler : IDataSourceItemHandler
    {
        public IEnumerable<DataSourceItem> GetData(DataSourceContext context)
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
