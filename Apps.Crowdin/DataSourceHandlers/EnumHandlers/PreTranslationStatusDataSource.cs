using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers;

public class PreTranslationStatusDataSource : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData()
    {
        return new()
        {
            { "created", "Created"},
            { "in_progress", "In progress"},
            { "canceled", "Canceled"},
            { "failed", "Failed"},
            { "finished", "Finished"},
        };
    }
}