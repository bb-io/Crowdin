using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Response.Directory
{
    public class GetDirectoryProgressResponse
    {
        [Display("Project ID")]
        public string ProjectId { get; set; } = default!;

        [Display("Directory ID")]
        public string DirectoryId { get; set; } = default!;

        [Display("Languages progress")]
        public IEnumerable<DirectoryLanguageProgressEntity> Languages { get; set; } = Array.Empty<DirectoryLanguageProgressEntity>();

        [Display("Total words (all languages)")]
        public int WordsTotal { get; set; }

        [Display("Translated words (all languages)")]
        public int WordsTranslated { get; set; }

        [Display("Pre-translated words (all languages)")]
        public int WordsPreTranslated { get; set; }

        [Display("Approved words (all languages)")]
        public int WordsApproved { get; set; }

        [Display("Total phrases (all languages)")]
        public int PhrasesTotal { get; set; }

        [Display("Translated phrases (all languages)")]
        public int PhrasesTranslated { get; set; }

        [Display("Pre-translated phrases (all languages)")]
        public int PhrasesPreTranslated { get; set; }

        [Display("Approved phrases (all languages)")]
        public int PhrasesApproved { get; set; }

        [Display("Languages count")]
        public int LanguagesCount { get; set; }

        [Display("Has untranslated content")]
        public bool HasUntranslated { get; set; }

        [Display("Has unapproved content")]
        public bool HasUnapproved { get; set; }
    }
}
