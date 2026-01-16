using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Crowdin.Polling.Models.Requests
{
    public class AllTasksReachedStatusRequest
    {
        [Display("Status", Description = "Desired statuses. Default is 'done'.")]
        [StaticDataSource(typeof(TaskStatusTypeHandler))]
        public IEnumerable<string>? Status { get; set; }

        [Display("Title contains")]
        public string? TitleContains { get; set; }
    }
}
