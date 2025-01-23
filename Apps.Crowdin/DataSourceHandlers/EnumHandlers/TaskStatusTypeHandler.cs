using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers
{
    public class TaskStatusTypeHandler : IStaticDataSourceItemHandler
    {
        private static Dictionary<string, string> Data => new()
        {
            { "todo", "To do" },
            { "in_progress", "In progress" },
            { "done", "Done" },
            { "closed", "Closed" }
        };

        public IEnumerable<DataSourceItem> GetData()
        {
            return Data.Select(x => new DataSourceItem(x.Key, x.Value));
        }
    }
}
