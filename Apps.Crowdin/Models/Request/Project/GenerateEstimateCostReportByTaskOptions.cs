using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.Project
{
    public class GenerateEstimateCostReportByTaskOptions : GenerateEstimateCostReportOptions
    {
        [Display("Task ID")]
        public string TaskId { get; set; }
    }

    public class GenerateTranslationCostReportByTaskOptions : GenerateTranslationCostReportOptions
    {
        [Display("Task ID")]
        public string TaskId { get; set; }
    }
}
