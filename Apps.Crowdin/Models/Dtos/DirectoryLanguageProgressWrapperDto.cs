namespace Apps.Crowdin.Models.Dtos
{
    public class DirectoryLanguageProgressWrapperDto
    {
        public DirectoryLanguageProgressDto Data { get; set; } = default!;
    }

    public class DirectoryLanguageProgressDto
    {
        public WordsPhrasesUnitDto Words { get; set; } = default!;
        public WordsPhrasesUnitDto Phrases { get; set; } = default!;

        public int TranslationProgress { get; set; }
        public int ApprovalProgress { get; set; }

        public string LanguageId { get; set; } = default!;
        public LanguageInfoDto? Language { get; set; }
    }

    public class WordsPhrasesUnitDto
    {
        public int Total { get; set; }
        public int Translated { get; set; }
        public int PreTranslateAppliedTo { get; set; }
        public int Approved { get; set; }
    }

    public class LanguageInfoDto
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string EditorCode { get; set; } = default!;
        public string TwoLettersCode { get; set; } = default!;
        public string ThreeLettersCode { get; set; } = default!;
        public string Locale { get; set; } = default!;
        public string AndroidCode { get; set; } = default!;
        public string OsxCode { get; set; } = default!;
        public string OsxLocale { get; set; } = default!;
        public string[] PluralCategoryNames { get; set; } = Array.Empty<string>();
        public string PluralRules { get; set; } = default!;
        public string[] PluralExamples { get; set; } = Array.Empty<string>();
        public string TextDirection { get; set; } = default!;
        public string? DialectOf { get; set; }
    }

    public class DirectoryProgressListResponseDto
    {
        public List<DirectoryLanguageProgressWrapperDto> Data { get; set; } = new();
        public PaginationDto Pagination { get; set; } = default!;
    }
}
