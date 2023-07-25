using Apps.Crowdin.Models.Entities;

namespace Apps.Crowdin.Models.Response.Translation;

public record ListTranslationsResponse(TranslationEntity[] Translations);