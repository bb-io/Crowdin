using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.Task;

public class GetTaskOptionalRequest
{
    [Display("Task ID")]
    public string? TaskId { get; set; }

    [Display("Status")]
    [StaticDataSource(typeof(TaskStatusTypeHandler))]
    public string? Status {  get; set; }

    [Display("Task type")]
    [StaticDataSource(typeof(TaskTypeWebhookHandler))]
    public string? Type { get; set; }
}