using Apps.Crowdin.Api;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.Project;
using Apps.Crowdin.Models.Request.Translation;
using Apps.Crowdin.Models.Response.File;
using Apps.Crowdin.Models.Response.Translation;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Parsers;
using Crowdin.Api.SourceFiles;
using Crowdin.Api.StringTranslations;
using Crowdin.Api.Translations;
using RestSharp;
using Apps.Crowdin.Models.Request.Users;

namespace Apps.Crowdin.Actions;

[ActionList]
public class TranslationActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : BaseInvocable(invocationContext)
{
    private AuthenticationCredentialsProvider[] Creds =>
        InvocationContext.AuthenticationCredentialsProviders.ToArray();

    [Action("Apply pre-translation", Description = "Apply pre-translation to chosen files")]
    public async Task<PreTranslationEntity> PreTranslate(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] PreTranslateRequest input,
        [ActionParameter] UserRequest user)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
        var intEngineId = IntParser.Parse(input.EngineId, nameof(input.EngineId));

        var client = new CrowdinClient(Creds);

        PreTranslationMethod? method = input.Method is null ? null :
            input.Method == "Mt" ? PreTranslationMethod.Mt :
            input.Method == "Tm" ? PreTranslationMethod.Tm :
            PreTranslationMethod.Ai;
        AutoApproveOption? option = input.AutoApproveOption is null ? null : input.AutoApproveOption == "None" ? AutoApproveOption.None : input.AutoApproveOption == "All" ? AutoApproveOption.All : input.AutoApproveOption == "ExceptAutoSubstituted" ? AutoApproveOption.ExceptAutoSubstituted : AutoApproveOption.PerfectMatchOnly;

        var request = new ApplyPreTranslationRequest
        {
            LanguageIds = input.LanguageIds.ToList(),
            FileIds = input.FileIds.Select(fileId => IntParser.Parse(fileId, nameof(fileId))!.Value).ToList(),
            EngineId = intEngineId,
            Method = method,
            AiPromptId = input.aiPromptId is null ? null : IntParser.Parse(input.aiPromptId, nameof(input.aiPromptId)),
            AutoApproveOption = option,
            DuplicateTranslations = input.DuplicateTranslations,
            TranslateUntranslatedOnly = input.TranslateUntranslatedOnly,
            TranslateWithPerfectMatchOnly = input.TranslateWithPerfectMatchOnly
        };
        var response = await client.Translations
            .ApplyPreTranslation(intProjectId!.Value, request);

        return new(response);
    }
    [Action("List language translations", Description = "List project language translations")]
    public async Task<ListTranslationsResponse> ListLangTranslations(
        [ActionParameter] ListLanguageTranslationsRequest input)
    {
        var intProjectId = IntParser.Parse(input.ProjectId, nameof(input.ProjectId));
        var intFileId = IntParser.Parse(input.FileId, nameof(input.FileId));
    
        var client = new CrowdinClient(Creds);
    
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
        [ActionParameter] ListStringTranslationsRequest input)
    {
        var intProjectId = IntParser.Parse(input.ProjectId, nameof(input.ProjectId));
        var intStringId = IntParser.Parse(input.StringId, nameof(input.StringId));

        var client = new CrowdinClient(Creds);

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
    public async Task<TranslationEntity> GetTranslation([ActionParameter] GetTranslationRequest input)
    {
        var intProjectId = IntParser.Parse(input.ProjectId, nameof(input.ProjectId));
        var intTransId = IntParser.Parse(input.TranslationId, nameof(input.TranslationId));

        var client = new CrowdinClient(Creds);

        var response = await client.StringTranslations
            .GetTranslation(intProjectId!.Value, intTransId!.Value, input.DenormalizePlaceholders);

        return new(response);
    }

    [Action("Add string translation", Description = "Add new string translation")]
    public async Task<TranslationEntity> AddStringTranslation([ActionParameter] AddNewTranslationRequest input)
    {
        var intProjectId = IntParser.Parse(input.ProjectId, nameof(input.ProjectId));
        var intStringId = IntParser.Parse(input.StringId, nameof(input.StringId));

        var client = new CrowdinClient(Creds);

        var request = new AddTranslationRequest
        {
            StringId = intStringId!.Value,
            LanguageId = input.LanguageId,
            Text = input.Text,
            PluralCategoryName = EnumParser.Parse<PluralCategoryName>(input.PluralCategoryName, nameof(input.PluralCategoryName))
        };
        
        var response = await client.StringTranslations.AddTranslation(intProjectId!.Value,request);
        return new(response);
    }

    [Action("Add file translation", Description = "Add new file translation")]
    public async Task<FileTranslationEntity> AddFileTranslation([ActionParameter] AddNewFileTranslationRequest input)
    {
        var intProjectId = IntParser.Parse(input.ProjectId, nameof(input.ProjectId));
        var client = new CrowdinClient(Creds);

        var fileStream = await fileManagementClient.DownloadAsync(input.File);
        var storageResult = await client.Storage.AddStorage(fileStream, input.File.Name);

        var request = new UploadTranslationsRequest
        {
            StorageId = storageResult.Id,
            FileId = int.Parse(input.SourceFileId),
            ImportEqSuggestions = input.ImportEqSuggestions,
            AutoApproveImported = input.AutoApproveImported,
            TranslateHidden = input.TranslateHidden
        };

        var response = await client.Translations.UploadTranslations(intProjectId!.Value, input.LanguageId, request);
        return new(response);
    }

    [Action("Delete translation", Description = "Delete specific translation")]
    public Task DeleteTranslation([ActionParameter] DeleteTranslationRequest input)
    {
        var intProjectId = IntParser.Parse(input.ProjectId, nameof(input.ProjectId));
        var intTransId = IntParser.Parse(input.TranslationId, nameof(input.TranslationId));

        var client = new CrowdinClient(Creds);

        return client.StringTranslations
            .DeleteTranslation(intProjectId!.Value, intTransId!.Value);
    }

    [Action("Download file translation", Description = "Builds and downloads the translation of a file")]
    public async Task<DownloadFileResponse> DownloadTranslationFile(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] DownloadFileTranslationRequest request
        )
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
        var intFileId = IntParser.Parse(request.FileId, nameof(request.FileId));

        var client = new CrowdinClient(Creds);

        var fileInfo = await client.SourceFiles.GetFile<FileResource>(intProjectId!.Value, intFileId!.Value);

        var build = await client.Translations.BuildProjectFileTranslation(intProjectId!.Value, intFileId!.Value, new BuildProjectFileTranslationRequest
        {
            TargetLanguageId = request.TargetLanguage,
            SkipUntranslatedStrings = request.SkipUntranslatedStrings,
            SkipUntranslatedFiles = request.SkipUntranslatedFiles,
            ExportApprovedOnly = request.ExportApprovedOnly,
        });

        if (!MimeTypes.TryGetMimeType(fileInfo.Name, out var contentType))
            contentType = "application/octet-stream";

        var bytes = new RestClient(build.Link.Url).Get(new RestRequest("/")).RawBytes;

        var file = await fileManagementClient.UploadAsync(new MemoryStream(bytes), contentType, fileInfo.Name);
        return new(file);
    }
}