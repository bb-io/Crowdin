using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Response.Project
{
    public class TranslationCostReportResponse
    {
        [Display("Total words")]
        public int? TotalWords { get; set; }

        [Display("Weighted words")]
        public decimal? WeightedWords { get; set; }

        [Display("Task name")]
        public string? TaskName { get; set; }

        [Display("Translation cost")]
        public decimal? TranslationCost { get; set; }

        [Display("Proofreading cost")]
        public decimal? ProofreadingCost { get; set; }

        [Display("Estimated TM savings total")]
        public decimal? EstimatedTMSavingsTotal { get; set; }
    }
}
