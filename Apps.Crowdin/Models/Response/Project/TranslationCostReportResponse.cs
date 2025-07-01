using Blackbird.Applications.Sdk.Common;
using Crowdin.Api.Reports;

namespace Apps.Crowdin.Models.Response.Project
{
    public class TranslationCostReportResponse
    {
        [Display("Total words")]
        public int? TotalWords { get; set; }

        [Display("Weighted words")]
        public decimal? WeightedWords { get; set; }

        [Display("Currency")]
        public string? Currency { get; set; }

        [Display("Task name")]
        public string? TaskName { get; set; }

        [Display("Translation cost")]
        public decimal? TranslationCost { get; set; }

        [Display("Proofreading cost")]
        public decimal? ProofreadingCost { get; set; }

        [Display("Estimated TM savings total")]
        public decimal? EstimatedTMSavingsTotal { get; set; }

        [Display("Columns")]
        public ColumnValue[]? Columns { get; set; }

        [Display("Details by language")]
        public LanguageCostDetail[]? Details { get; set; }

        [Display("Base rates used")]
        public BaseRates? BaseRates { get; set; }


        [Display("Savings breakdown")]
        public ColumnValue[]? Savings { get; set; }

        [Display("Weighted units breakdown")]
        public ColumnValue[]? WeightedUnits { get; set; }

        [Display("Pre-translated breakdown")]
        public ColumnValue[]? PreTranslated { get; set; }

        [Display("Translation costs breakdown")]
        public ColumnValue[]? TranslationCosts { get; set; }


        //total words array of columns
        //give the input of language ids
        //currency
        //Base rates optional input
    }

    public class BaseRates
    {
        [Display("Full translation rate")]
        public decimal FullTranslation { get; set; }

        [Display("Proofread rate")]
        public decimal Proofread { get; set; }
    }

    public class ReportColumn
    {
        [Display("Title")]
        public string Title { get; set; } = string.Empty;

        [Display("Type")]
        public string Type { get; set; } = string.Empty;
    }

    public class LanguageCostDetail
    {
        [Display("Language ID")]
        public string LanguageId { get; set; } = string.Empty;

        [Display("Total words")]
        public int TotalWords { get; set; }

        [Display("Weighted words")]
        public decimal WeightedWords { get; set; }

        [Display("Translation cost")]
        public decimal TranslationCost { get; set; }

        [Display("Proofreading cost")]
        public decimal ProofreadingCost { get; set; }

        [Display("TM savings")]
        public decimal TMSavings { get; set; }
    }

    public class ColumnValue
    {
        [Display("Name")]
        public string Name { get; set; } = string.Empty;

        [Display("Value")]
        public decimal Value { get; set; }
    }
}
