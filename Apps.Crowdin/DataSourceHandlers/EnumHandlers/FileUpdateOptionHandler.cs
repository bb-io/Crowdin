using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers;

public class FileUpdateOptionHandler : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData() => new()
    {
        { "clear_translations_and_approvals", "Clear translations and approvals" },
        { "keep_translations", "Keep translations" },
        { "keep_translations_and_approvals", "Keep translations and approvals" },
    };
}