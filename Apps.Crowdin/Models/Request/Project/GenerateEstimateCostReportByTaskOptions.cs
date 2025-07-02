using Apps.Crowdin.DataSourceHandlers;
using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.Project
{
    public class GenerateEstimateCostReportByTaskOptions
    {
        [Display("Base rates full translations")]
        public float? BaseFullTranslations { get; set; } = 0.10f;

        [Display("Base rates proofread")]
        public float? BaseProofRead { get; set; } = 0.05f;

        [Display("Currency")]
        [StaticDataSource(typeof(CurrencyDataHandler))]
        public string? Currency { get; set; }

        [Display("Unit")]
        [StaticDataSource(typeof(ReportFormatUnitDataHandler))]
        public string? Unit { get; set; } = "words";

        [Display("Language IDs")]
        [DataSource(typeof(LanguagesDataHandler))]
        public IEnumerable<string>? LanguageIds { get; set; }

        [Display("Individual rates full translations")]
        public float? IndividualFullTranslations { get; set; } = 0.10f;

        [Display("Individual rates proofread")]
        public float? IndividualProofRead { get; set; } = 0.05f;

        [Display("TM match type")]
        public string? TmMatchType { get; set; } = "100";

        [Display("TM price")]
        public float? TmPrice { get; set; } = 0.0f;

        [Display("MT match type")]
        public string? MtMatchType { get; set; } = "100";

        [Display("MT price")]
        public float? MtPrice { get; set; } = 0.0f;

        [Display("Task ID")]
        public string TaskId { get; set; }
    }

    public class GenerateTranslationCostReportByTaskOptions 
    {
        [Display("Currency")]
        [StaticDataSource(typeof(CurrencyDataHandler))]
        public string? Currency { get; set; }

        [Display("Unit")]
        [StaticDataSource(typeof(ReportFormatUnitDataHandler))]
        public string? Unit { get; set; } = "words";

        [Display("Base rates full translations")]
        public float? BaseFullTranslations { get; set; } = 0.10f;

        [Display("Base rates proofread")]
        public float? BaseProofRead { get; set; } = 0.05f;

        [Display("Language IDs")]
        [DataSource(typeof(LanguagesDataHandler))]
        public IEnumerable<string>? LanguageIds { get; set; }

        [Display("User IDs")]
        [DataSource(typeof(ProjectMemberDataSourceHandler))]
        public IEnumerable<string>? UserIds { get; set; }

        [Display("Individual rates full translations")]
        public float? IndividualFullTranslations { get; set; } = 0.10f;

        [Display("Individual rates proofread")]
        public float? IndividualProofRead { get; set; } = 0.05f;

        [Display("TM match type")]
        public string? TmMatchType { get; set; } = "100";

        [Display("TM price")]
        public float? TmPrice { get; set; } = 0.0f;

        [Display("MT match type")]
        public string? MtMatchType { get; set; } = "100";

        [Display("MT price")]
        public float? MtPrice { get; set; } = 0.0f;

        [Display("Suggestion match type")]
        public string? SuggestMatchType { get; set; } = "100";

        [Display("Suggestion price")]
        public float? SuggestPrice { get; set; } = 0.0f;

        [Display("Task ID")]
        public string TaskId { get; set; }
    }
}
