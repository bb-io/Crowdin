using Apps.Crowdin.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.Project
{
    public class GenerateEstimateCostReportOptions
    {
        [Display("Name")]
        public string Name { get; set; } = "costs-estimation-pe";

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
        public string TmMatchType { get; set; } = "perfect";

        [Display("TM price")]
        public float? TmPrice { get; set; } = 0.02f;

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
