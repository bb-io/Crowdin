using Apps.Crowdin.Api;
using Apps.Crowdin.Constants;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.Translation;
using Apps.Crowdin.Models.Response.Translation;
using Apps.Crowdin.Utils;
using Apps.Crowdin.Utils.Parsers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Crowdin.Api.StringTranslations;
using Crowdin.Api.Translations;

namespace Apps.Crowdin.Actions;

[ActionList]
public class TranslationActions
{
    [Action("Apply pre-translation", Description = "Apply pre-translation to chosen files")]
    public async Task<PreTranslationEntity> PreTranslate(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] [Display("Project ID")]
        string projectId,
        [ActionParameter] PreTranslateRequest input)
    {
        var intProjectId = IntParser.Parse(projectId, nameof(projectId));
        var intEngineId = IntParser.Parse(input.EngineId, nameof(input.EngineId));

        var client = new CrowdinClient(creds);

        var request = new ApplyPreTranslationRequest
        {
            LanguageIds = input.LanguageIds.ToList(),
            FileIds = input.FileIds.Select(fileId => IntParser.Parse(fileId, nameof(fileId))!.Value).ToList(),
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
    [Action("List language translations", Description = "List project language translations")]
    public async Task<ListTranslationsResponse> ListLangTranslations(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] ListLanguageTranslationsRequest input)
    {
        var intProjectId = IntParser.Parse(input.ProjectId, nameof(input.ProjectId));
        var intFileId = IntParser.Parse(input.FileId, nameof(input.FileId));
    
        var client = new CrowdinClient(creds);
    
        var items = await Paginator.Paginate((lim, offset) =>
        {
            var request = new LanguageTranslationsListParams
            {
                Limit = lim,
                Offset = offset,
                StringIds = input.StringIds,
                LabelIds = input.LabelIds,
                FileId = intFileId,
                CroQL = input.CroQL,
                DenormalizePlaceholders = input.DenormalizePlaceholders
            };
            return client.StringTranslations.ListLanguageTranslations(intProjectId!.Value, input.LanguageId, request);
        });

        var castedItems = items.Cast<PlainLanguageTranslations>();
       
        var translations = castedItems.Select(x => new TranslationEntity(x)).ToArray();
        return new(translations);
    }

    [Action("List string translations", Description = "List project string translations")]
    public async Task<ListTranslationsResponse> ListTranslations(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] ListStringTranslationsRequest input)
    {
        var intProjectId = IntParser.Parse(input.ProjectId, nameof(input.ProjectId));
        var intStringId = IntParser.Parse(input.StringId, nameof(input.StringId));

        var client = new CrowdinClient(creds);

        var items = await Paginator.Paginate((lim, offset) =>
        {
            var request = new StringTranslationsListParams
            {
                Limit = lim,
                Offset = offset,
                StringId = intStringId!.Value,
                LanguageId = input.LanguageId,
                DenormalizePlaceholders = input.DenormalizePlaceholders
            };
            return client.StringTranslations.ListStringTranslations(intProjectId!.Value, request);
        });

        var translations = items.Select(x => new TranslationEntity(x)).ToArray();
        return new(translations);
    }

    [Action("Get translation", Description = "Get specific translation")]
    public async Task<TranslationEntity> GetTranslation(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] GetTranslationRequest input)
    {
        var intProjectId = IntParser.Parse(input.ProjectId, nameof(input.ProjectId));
        var intTransId = IntParser.Parse(input.TranslationId, nameof(input.TranslationId));

        var client = new CrowdinClient(creds);

        var response = await client.StringTranslations
            .GetTranslation(intProjectId!.Value, intTransId!.Value, input.DenormalizePlaceholders);

        return new(response);
    }

    [Action("Add translation", Description = "Add new translation")]
    public async Task<TranslationEntity> AddTranslation(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] AddNewTranslationRequest input)
    {
        var intProjectId = IntParser.Parse(input.ProjectId, nameof(input.ProjectId));
        var intStringId = IntParser.Parse(input.StringId, nameof(input.StringId));

        var client = new CrowdinClient(creds);

        var request = new AddTranslationRequest
        {
            StringId = intStringId!.Value,
            LanguageId = input.LanguageId,
            Text = input.Text,
            PluralCategoryName = EnumParser.Parse<PluralCategoryName>(input.PluralCategoryName, nameof(input.PluralCategoryName), EnumValues.PluralCategoryName)
        };
        
        var response = await client.StringTranslations.AddTranslation(intProjectId!.Value,request);
        return new(response);
    }
    
    [Action("Delete translation", Description = "Delete specific translation")]
    public Task DeleteTranslation(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] DeleteTranslationRequest input)
    {
        var intProjectId = IntParser.Parse(input.ProjectId, nameof(input.ProjectId));
        var intTransId = IntParser.Parse(input.TranslationId, nameof(input.TranslationId));

        var client = new CrowdinClient(creds);

        return client.StringTranslations
            .DeleteTranslation(intProjectId!.Value, intTransId!.Value);
    }
}