using Blackbird.Applications.Sdk.Common;
using Crowdin.Api.Translations;

namespace Apps.Crowdin.Models.Entities;

public class FileTranslationEntity(TranslationImportResponse fileTranslation, string? projectId)
{
    [Display("File ID")]
    public string FileId { get; set; } = fileTranslation.Attributes.FileId.ToString();

    [Display("Language ID")]
    public string LanguageId { get; set; } = fileTranslation.Attributes.LanguageIds.First().ToString();

    [Display("Project ID")]
    public string? ProjectId { get; set; } = projectId;

    [Display("Storage ID")]
    public string StorageId { get; set; } = fileTranslation.Attributes.StorageId.ToString();
}