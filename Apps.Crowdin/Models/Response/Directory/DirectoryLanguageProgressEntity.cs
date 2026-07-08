using Apps.Crowdin.Models.Dtos;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Response.Directory
{
    public class DirectoryLanguageProgressEntity
    {
        public DirectoryLanguageProgressEntity(DirectoryLanguageProgressDto src)
        {
            LanguageId = src.LanguageId;
            LanguageName = src.Language?.Name;
            LanguageLocale = src.Language?.Locale;
            LanguageTwoLettersCode = src.Language?.TwoLettersCode;
            LanguageThreeLettersCode = src.Language?.ThreeLettersCode;

            TranslationProgress = src.TranslationProgress;
            ApprovalProgress = src.ApprovalProgress;

            WordsTotal = src.Words?.Total ?? 0;
            WordsTranslated = src.Words?.Translated ?? 0;
            WordsPreTranslated = src.Words?.PreTranslateAppliedTo ?? 0;
            WordsApproved = src.Words?.Approved ?? 0;

            PhrasesTotal = src.Phrases?.Total ?? 0;
            PhrasesTranslated = src.Phrases?.Translated ?? 0;
            PhrasesPreTranslated = src.Phrases?.PreTranslateAppliedTo ?? 0;
            PhrasesApproved = src.Phrases?.Approved ?? 0;
        }

        [Display("Language ID")]
        public string LanguageId { get; set; } = default!;

        [Display("Language name")]
        public string? LanguageName { get; set; }

        [Display("Locale")]
        public string? LanguageLocale { get; set; }

        [Display("Two letters code")]
        public string? LanguageTwoLettersCode { get; set; }

        [Display("Three letters code")]
        public string? LanguageThreeLettersCode { get; set; }

        [Display("Translation progress (%)")]
        public int TranslationProgress { get; set; }

        [Display("Approval progress (%)")]
        public int ApprovalProgress { get; set; }

        [Display("Total words")]
        public int WordsTotal { get; set; }

        [Display("Translated words")]
        public int WordsTranslated { get; set; }

        [Display("Pre-translated words")]
        public int WordsPreTranslated { get; set; }

        [Display("Approved words")]
        public int WordsApproved { get; set; }

        [Display("Total phrases")]
        public int PhrasesTotal { get; set; }

        [Display("Translated phrases")]
        public int PhrasesTranslated { get; set; }

        [Display("Pre-translated phrases")]
        public int PhrasesPreTranslated { get; set; }

        [Display("Approved phrases")]
        public int PhrasesApproved { get; set; }
    }
}
