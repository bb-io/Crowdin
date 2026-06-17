using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.Project;

public class ProjectReportRequest
{
    [Display("Report ID")]
    public string ReportId { get; set; } = string.Empty;
}