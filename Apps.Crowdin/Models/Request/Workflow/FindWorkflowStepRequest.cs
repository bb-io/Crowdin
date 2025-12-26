using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Crowdin.Models.Request.Workflow
{
    public class FindWorkflowStepRequest
    {
        [Display("Workflow step type")]
        [StaticDataSource(typeof(WorkflowStepTypeHandler))]
        public string Type { get; set; } = string.Empty;

        [Display("Workflow step title (optional)")]
        public string? Title { get; set; }

        [Display("Title contains (optional)", Description = "If true, Title filter will be applied as 'contains' instead of exact match")]
        public bool? TitleContains { get; set; }
    }
}
