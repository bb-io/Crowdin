using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers;

public class StringScopeHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        { "Identifier", "Identifier" },
        { "Text", "Text" },
        { "Context", "Context" },
    };
}