using Apps.Crowdin.DataSourceHandlers;
using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.Project
{
    public class GenerateEstimateCostReportOptions
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
        public IEnumerable<string> LanguageIds { get; set; }

        [Display("Individual rates full translations")]
        public float? IndividualFullTranslations { get; set; } = 0.10f;

        [Display("Individual rates proofread")]
        public float? IndividualProofRead { get; set; } = 0.05f;

        [Display("TM match type")]
        public string? TmMatchType { get; set; } = "perfect";

        [Display("TM price")]
        public float? TmPrice { get; set; } = 0.02f;


        [Display("From date")]
        public DateTime? FromDate { get; set; }

        [Display("To date")]
        public DateTime? ToDate { get; set; }
    }
}
