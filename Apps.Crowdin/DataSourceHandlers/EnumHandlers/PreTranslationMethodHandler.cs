using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers
{
    public class PreTranslationMethodHandler : IStaticDataSourceItemHandler
    {
        private static Dictionary<string, string> Data => new()
        {
            { "Mt", "Machine translation" },
            { "Tm", "Translation memory" },
            { "Ai", "Artificial intelligence" }
        };
            
        public IEnumerable<DataSourceItem> GetData()
        {
            return Data.Select(x => new DataSourceItem(x.Key, x.Value));
        }
    }
}
