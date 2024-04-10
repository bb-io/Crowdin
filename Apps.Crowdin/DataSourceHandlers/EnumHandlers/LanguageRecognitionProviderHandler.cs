using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers;

public class LanguageRecognitionProviderHandler : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData() => new()
    {
        { "Crowdin", "Crowdin" },
        { "Engine", "Engine" },
    };
}