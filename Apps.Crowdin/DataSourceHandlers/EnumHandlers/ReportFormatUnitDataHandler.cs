using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers
{
    public class ReportFormatUnitDataHandler : IStaticDataSourceItemHandler
    {
        private static readonly Dictionary<string, string> Data = new()
        {
            { "strings", "Strings" },
            { "words", "Words" },
            { "chars", "Chars" },
            { "chars_with_spaces", "Chars With Spaces" }
        };

        public IEnumerable<DataSourceItem> GetData()
            => Data.Select(x => new DataSourceItem(x.Key, x.Value));
    }
}
