using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers
{
    public class ExportFileFormatHandler : IStaticDataSourceItemHandler
    {
        private static Dictionary<string, string> Data => new()
        {
            { "android", "Android" },
            { "macosx", "macOS" },
            { "xliff", "XLIFF" }
        };

        public IEnumerable<DataSourceItem> GetData()
        {
            return Data.Select(x => new DataSourceItem(x.Key, x.Value));
        }
    }
}
