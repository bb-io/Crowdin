using Apps.Crowdin.DataSourceHandlers;
using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Polling.Models.Requests
{
    public class AllTasksReachedStatusRequest
    {
        [Display("Project ID")]
        [DataSource(typeof(ProjectDataHandler))]
        public string ProjectId { get; set; }

        [Display("Status", Description = "Desired statuses. Default is 'done'.")]
        [StaticDataSource(typeof(TaskStatusTypeHandler))]
        public IEnumerable<string>? Status { get; set; }        

        [Display("Title contains")]
        public string? TitleContains { get; set; }
    }
}
