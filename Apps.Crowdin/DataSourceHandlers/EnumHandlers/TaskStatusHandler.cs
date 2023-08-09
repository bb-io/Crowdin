using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers;

public class TaskStatusHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        {"Todo", "To do"},
        {"InProgress", "In progress"},
        {"Done", "Done"},
        {"Closed", "Closed"},
    };
}