using Apps.Crowdin.DataSourceHandlers;
using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.Task;

public class AddNewVendorTaskRequest
{
    [Display("Workflow step ID"), DataSource(typeof(WorkflowStepDataHandler))]
    public string WorkflowStepId { get; set; }
    
    public string Title { get; set; }

    public string? Description { get; set; }

    [Display("Language ID"), DataSource(typeof(LanguagesDataHandler))]
    public string LanguageId { get; set; }

    [Display("File IDs")]
    public IEnumerable<string> FileIds { get; set; }

    [Display("Status"), StaticDataSource(typeof(VendorTaskStatusTypeHandler))]
    public string? Status { get; set; }

    [Display("Deadline")]
    public DateTime? Deadline { get; set; }

    [Display("Date from")]
    public DateTime? DateFrom { get; set; }

    [Display("Date to")]
    public DateTime? DateTo { get; set; }
}
