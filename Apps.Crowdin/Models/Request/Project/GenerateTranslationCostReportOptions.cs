using Apps.Crowdin.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.Project
{
    public class GenerateTranslationCostReportOptions
    {
        [Display("Base rates full translations")]
        public float? BaseFullTranslations { get; set; } = 0.10f;

        [Display("Base rates proofread")]
        public float? BaseProofRead { get; set; } = 0.0f;

        [Display("Currency")]
        public string? Currency { get; set; }

        [Display("Language IDs")]
        [DataSource(typeof(LanguagesDataHandler))]
        public IEnumerable<string>? LanguageIds { get; set; }

        [Display("User IDs")]
        [DataSource(typeof(ProjectMemberDataSourceHandler))]
        public IEnumerable<string>? UserIds { get; set; }

        [Display("Individual rates full translations")]
        public float? IndividualFullTranslations { get; set; } = 0.10f;

        [Display("Individual rates proofread")]
        public float? IndividualProofRead { get; set; } = 0.0f;

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

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
