using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers;

public class TaskStatusHandler : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData() => new()
    {
        { "Todo", "To do" },
        { "InProgress", "In progress" },
        { "Done", "Done" },
        { "Closed", "Closed" },
    };
}