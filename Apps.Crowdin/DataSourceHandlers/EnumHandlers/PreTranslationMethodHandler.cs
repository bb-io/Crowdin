using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers
{
    public class PreTranslationMethodHandler : IStaticDataSourceHandler
    {
            public Dictionary<string, string> GetData() => new()
        {
            { "Mt", "Machine translation" },
            { "Tm", "Translation memory" },
        };
    }
}
