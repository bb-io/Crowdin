using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers;

public class UserRoleHandler : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData() => new()
    {
        {"all", "All"},
        {"manager", "Manager"},
        {"developer", "Developer"},
        {"language_coordinator", "Language Coordinator"},
        {"proofreader", "Proofreader"},
        {"translator", "Translator"},
        {"blocked", "Blocked"},
        {"pending", "Pending"}
    };
}