using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers;

public class LanguageRecognitionProviderHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        { "Crowdin", "Crowdin" },
        { "Engine", "Engine" },
    };
}