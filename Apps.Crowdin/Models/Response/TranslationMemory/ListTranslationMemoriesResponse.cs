using Apps.Crowdin.Models.Entities;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Response.TranslationMemory;

public record ListTranslationMemoriesResponse([property: Display("Translation memories")]
    TranslationMemoryEntity[] TranslationMemories);