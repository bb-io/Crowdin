using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Response.Project
{
    public class EstimateCostReportResponse
    {

        [Display("Unit")] public string? Unit { get; set; }
        [Display("Currency")] public string? Currency { get; set; }
        [Display("Calculate Internal Matches")]
        public bool CalculateInternalMatches { get; set; }
        [Display("Approval rate")] public decimal? ApprovalRate { get; set; }
        [Display("Generated at")] public string? GeneratedAt { get; set; }
        [Display("Task ID")] public int? TaskId { get; set; }
        [Display("Task title")] public string? TaskTitle { get; set; }
        [Display("Language ID")] public string? LanguageId { get; set; }
        [Display("Language name")] public string? LanguageName { get; set; }

        [Display("Matches breakdown")]
        public ColumnValue[]? Matches { get; set; }

        [Display("Translation rates breakdown")]
        public ColumnValue[]? TranslationRates { get; set; }

        [Display("Translation costs breakdown")]
        public ColumnValue[]? TranslationCosts { get; set; }

        [Display("Approval costs breakdown")]
        public ColumnValue[]? ApprovalCosts { get; set; }

        [Display("Savings breakdown")]
        public ColumnValue[]? Savings { get; set; }

        [Display("Weighted units breakdown")]
        public ColumnValue[]? WeightedUnits { get; set; }

        [Display("Files details")]
        public FileEstimateDetail[]? Files { get; set; }
    }

    public class FileEstimateDetail
    {
        [Display("File ID")] public int FileId { get; set; }
        [Display("Path")] public string Path { get; set; } = string.Empty;
        [Display("Untranslated")] public int Untranslated { get; set; }
        [Display("Unapproved")] public int Unapproved { get; set; }

        [Display("Matches")] public ColumnValue[] Matches { get; set; } = Array.Empty<ColumnValue>();
        [Display("Translation costs")] public ColumnValue[] TranslationCosts { get; set; } = Array.Empty<ColumnValue>();
        [Display("Approval costs")] public ColumnValue[] ApprovalCosts { get; set; } = Array.Empty<ColumnValue>();
        [Display("Savings")] public ColumnValue[] Savings { get; set; } = Array.Empty<ColumnValue>();
        [Display("Weighted units")] public ColumnValue[] WeightedUnits { get; set; } = Array.Empty<ColumnValue>();

        [Display("Detailed stats untranslated")]
        public DetailedValue DetailedUntranslated { get; set; } = new();
        [Display("Detailed stats unapproved")]
        public DetailedValue DetailedUnapproved { get; set; } = new();

        [Display("Detailed matches")] public ColumnValue[] DetailedMatches { get; set; } = Array.Empty<ColumnValue>();
        [Display("Detailed weighted units")] public ColumnValue[] DetailedWeightedUnits { get; set; } = Array.Empty<ColumnValue>();
    }

    public class DetailedValue
    {
        [Display("Strings")] public int Strings { get; set; }
        [Display("Words")] public int Words { get; set; }
        [Display("Chars")] public int Chars { get; set; }
        [Display("Chars with spaces")] public int CharsWithSpaces { get; set; }
    }
}
