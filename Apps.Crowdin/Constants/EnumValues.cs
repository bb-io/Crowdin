namespace Apps.Crowdin.Constants;

public static class EnumValues
{
    public static readonly string[] TaskStatus = { "Todo", "InProgress", "Done", "Closed" };
    public static readonly string[] TmFileFormat = { "Tmx", "Csv", "Xlsx" };
    public static readonly string[] LanguageRecognitionProvider = { "Crowdin", "Engine" };
    public static readonly string[] TaskType = { "Translate", "Proofread", "TranslateByVendor", "ProofreadByVendor" };
    public static readonly string[] StringCommentType = { "Comment", "Issue" };
    public static readonly string[] IssueStatus = { "Resolved", "Unresolved" };
    public static readonly string[] IssueType = { "GeneralQuestion", "TranslationMistake", "ContextRequest", "SourceMistake" };
    public static readonly string[] PluralCategoryName = { "Zero", "One", "Two", "Few", "Many", "Other" };
    public static readonly string[] StringScope = { "Identifier", "Text", "Context" };
    public static readonly string[] ProjectVisibility = { "Open", "Private" };
}