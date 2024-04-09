using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers;

public class FileUpdateOptionHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        { "clear_translations_and_approvals", "Clear translations and approvals" },
        { "keep_translations", "Keep translations" },
        { "keep_translations_and_approvals", "Keep translations and approvals" },
    };
}