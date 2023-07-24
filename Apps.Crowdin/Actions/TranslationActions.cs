using Apps.Crowdin.Api;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.Translation;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Crowdin.Api.Translations;

namespace Apps.Crowdin.Actions;

[ActionList]
public class TranslationActions
{
    [Action("Apply pre-translation", Description = "Apply pre-translation to chosen files")]
    public async Task<PreTranslationEntity> PreTranslate(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] [Display("Project ID")] string projectId,
        [ActionParameter] PreTranslateRequest input)
    {
        var intProjectId = IntParser.Parse(projectId, nameof(projectId));
        var intEngineId = IntParser.Parse(input.EngineId, nameof(input.EngineId));
        
        var client = new CrowdinClient(creds);

        var request = new ApplyPreTranslationRequest
        {
            LanguageIds = input.LanguageIds.ToList(),
            FileIds = input.FileIds.ToList(),
            EngineId = intEngineId,
            DuplicateTranslations = input.DuplicateTranslations,
            TranslateUntranslatedOnly = input.TranslateUntranslatedOnly,
            TranslateWithPerfectMatchOnly = input.TranslateWithPerfectMatchOnly,
            MarkAddedTranslationsAsDone = input.MarkAddedTranslationsAsDone
        };
        var response = await client.Translations
            .ApplyPreTranslation(intProjectId!.Value, request);
        
        return new(response);
    }
}