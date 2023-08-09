using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers;

public class IssueTypeHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        { "GeneralQuestion", "General question" },
        { "TranslationMistake", "Translation mistake" },
        { "ContextRequest", "Context request" },
        { "SourceMistake", "Source mistake" },
    };
}