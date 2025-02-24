using Apps.Crowdin.Constants;
using Apps.Crowdin.Invocables;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.Project;
using Apps.Crowdin.Models.Request.Translation;
using Apps.Crowdin.Models.Response.File;
using Apps.Crowdin.Models.Response.Translation;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Parsers;
using Crowdin.Api.SourceFiles;
using Crowdin.Api.StringTranslations;
using Crowdin.Api.Translations;
using RestSharp;
using Apps.Crowdin.Models.Request.Users;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Crowdin.Api;
using Apps.Crowdin.Api.RestSharp.Enterprise;
using Apps.Crowdin.Api.RestSharp;
using Newtonsoft.Json;

namespace Apps.Crowdin.Actions;

[ActionList]
public class TranslationActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : AppInvocable(invocationContext)
{
    [Action("Apply pre-translation", Description = "Apply pre-translation to chosen files")]
    public async Task<PreTranslationEntity> PreTranslate(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] PreTranslateRequest input,
        [ActionParameter] UserRequest user)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
        var intEngineId = IntParser.Parse(input.EngineId, nameof(input.EngineId));

        PreTranslationMethod? method = input.Method is null ? null :
            input.Method == "Mt" ? PreTranslationMethod.Mt :
            input.Method == "Tm" ? PreTranslationMethod.Tm :
            PreTranslationMethod.Ai;
        AutoApproveOption? option = input.AutoApproveOption is null ? null :
            input.AutoApproveOption == "None" ? AutoApproveOption.None :
            input.AutoApproveOption == "All" ? AutoApproveOption.All :
            input.AutoApproveOption == "ExceptAutoSubstituted" ? AutoApproveOption.ExceptAutoSubstituted :
            AutoApproveOption.PerfectMatchOnly;

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

        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () => await SdkClient.Translations
            .ApplyPreTranslation(intProjectId!.Value, request));

        return new(response);
    }

    [Action("Search language translations", Description = "List project language translations")]
    public async Task<ListTranslationsResponse> ListLangTranslations(
        [ActionParameter] ListLanguageTranslationsRequest input)
    {
        var intProjectId = IntParser.Parse(input.ProjectId, nameof(input.ProjectId));
        var intFileId = IntParser.Parse(input.FileId, nameof(input.FileId));

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
            return ExceptionWrapper.ExecuteWithErrorHandling(() =>
                SdkClient.StringTranslations.ListLanguageTranslations(intProjectId!.Value, input.LanguageId, request));
        });

        var castedItems = items.Cast<PlainLanguageTranslations>();

        var translations = castedItems.Select(x => new TranslationEntity(x)).ToArray();
        return new(translations);
    }

    [Action("Search string translations", Description = "List project string translations")]
    public async Task<ListTranslationsResponse> ListTranslations(
        [ActionParameter] ListStringTranslationsRequest input)
    {
        var intProjectId = IntParser.Parse(input.ProjectId, nameof(input.ProjectId));
        var intStringId = IntParser.Parse(input.StringId, nameof(input.StringId));

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
            return ExceptionWrapper.ExecuteWithErrorHandling(() =>
                SdkClient.StringTranslations.ListStringTranslations(intProjectId!.Value, request));
        });

        var translations = items.Select(x => new TranslationEntity(x)).ToArray();
        return new(translations);
    }

    [Action("Get translation", Description = "Get specific translation")]
    public async Task<TranslationEntity> GetTranslation([ActionParameter] GetTranslationRequest input)
    {
        var intProjectId = IntParser.Parse(input.ProjectId, nameof(input.ProjectId));
        var intTransId = IntParser.Parse(input.TranslationId, nameof(input.TranslationId));

        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () => await SdkClient.StringTranslations
            .GetTranslation(intProjectId!.Value, intTransId!.Value, input.DenormalizePlaceholders));

        return new(response);
    }

    [Action("Add string translation", Description = "Add new string translation")]
    public async Task<TranslationEntity> AddStringTranslation([ActionParameter] AddNewTranslationRequest input)
    {
        var intProjectId = IntParser.Parse(input.ProjectId, nameof(input.ProjectId));
        var intStringId = IntParser.Parse(input.StringId, nameof(input.StringId));

        var request = new AddTranslationRequest
        {
            StringId = intStringId!.Value,
            LanguageId = input.LanguageId,
            Text = input.Text,
            PluralCategoryName =
                EnumParser.Parse<PluralCategoryName>(input.PluralCategoryName, nameof(input.PluralCategoryName))
        };

        try
        {
            var response = await SdkClient.StringTranslations.AddTranslation(intProjectId!.Value, request);
            return new(response);
        }
        catch (CrowdinApiException ex)
        {
            if (!ex.Message.Contains(Errors.IdenticalTranslation))
            {
                throw new PluginMisconfigurationException(ex.Message);
            }

            var translations = await ListTranslations(new()
            {
                ProjectId = input.ProjectId,
                StringId = input.StringId,
                LanguageId = input.LanguageId
            });

            return translations.Translations.First(x => x.Text == input.Text);
        }
    }

    [Action("Add file translation", Description = "Add new file translation")]
    public async Task<FileTranslationEntity> AddFileTranslation([ActionParameter] AddNewFileTranslationRequest input)
    {
        if (string.IsNullOrEmpty(input.LanguageId))
        {
            throw new PluginMisconfigurationException(
                "Language ID cannot be null or empty. Please provide a valid language ID.");
        }

        if (string.IsNullOrEmpty(input.ProjectId))
        {
            throw new PluginMisconfigurationException(
                "Project ID cannot be null or empty. Please provide a valid project ID.");
        }

        int? fileID;
        if (!String.IsNullOrEmpty(input.SourceFileId))
        {
            try
            {
                fileID = int.Parse(input.SourceFileId);
            }
            catch
            {
                throw new PluginMisconfigurationException("File ID is incorrect. Please check the input values.");
            }
        }
        else
        {
            if (input.File.Name.ToLower().EndsWith(".xliff") || input.File.Name.ToLower().EndsWith(".xlf"))
            {
                fileID = null;
            } else
            throw new PluginMisconfigurationException("File ID is required for all formats except XLIFF");
        }

        var intProjectId = IntParser.Parse(input.ProjectId, nameof(input.ProjectId));
        var client = SdkClient;

        var fileStream = await FileOperationWrapper.ExecuteFileDownloadOperation(
            () => fileManagementClient.DownloadAsync(input.File), input.File.Name);

        var memoryStream = new MemoryStream();
        await fileStream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        
        var storageResult = await ExceptionWrapper.ExecuteWithErrorHandling(async () => 
            await client.Storage.AddStorage(memoryStream, input.File.Name));

             
        var request = new UploadTranslationsRequest
        {
            StorageId = storageResult.Id,
            FileId = fileID,
            ImportEqSuggestions = input.ImportEqSuggestions,
            AutoApproveImported = input.AutoApproveImported,
            TranslateHidden = input.TranslateHidden
        };

        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
            await client.Translations.UploadTranslations(intProjectId!.Value, input.LanguageId, request));
        return new(response);
    }

    [Action("Delete translation", Description = "Delete specific translation")]
    public async Task DeleteTranslation([ActionParameter] DeleteTranslationRequest input)
    {
        var intProjectId = IntParser.Parse(input.ProjectId, nameof(input.ProjectId));
        var intTransId = IntParser.Parse(input.TranslationId, nameof(input.TranslationId));

        await ExceptionWrapper.ExecuteWithErrorHandling(async () => await SdkClient.StringTranslations
            .DeleteTranslation(intProjectId!.Value, intTransId!.Value));
    }

    [Action("Download file translation", Description = "Builds and downloads the translation of a file")]
    public async Task<DownloadFileResponse> DownloadTranslationFile(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] DownloadFileTranslationRequest request
    )
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
        var intFileId = IntParser.Parse(request.FileId, nameof(request.FileId));

        var client = SdkClient;

        var fileInfo = await ExceptionWrapper.ExecuteWithErrorHandling(async () => 
            await client.SourceFiles.GetFile<FileResource>(intProjectId!.Value, intFileId!.Value));
        
        var build = await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
            await client.Translations.BuildProjectFileTranslation(intProjectId!.Value, intFileId!.Value,
                new BuildProjectFileTranslationRequest
                {
                    TargetLanguageId = request.TargetLanguage,
                    SkipUntranslatedStrings = request.SkipUntranslatedStrings,
                    SkipUntranslatedFiles = request.SkipUntranslatedFiles,
                    ExportApprovedOnly = request.ExportApprovedOnly,
                }));

        if (!MimeTypes.TryGetMimeType(fileInfo.Name, out var contentType))
            contentType = "application/octet-stream";

        var bytes = new RestClient(build.Link.Url).Get(new RestRequest("/")).RawBytes;

        var file = await fileManagementClient.UploadAsync(new MemoryStream(bytes), contentType, fileInfo.Name);
        return new(file);
    }



    [Action("Get language progress", Description = "Get translation progress for a specific language in the project")]
    public async Task<Apps.Crowdin.Models.Response.Translation.LanguageProgressResponseDto> GetLanguageProgress(
    [ActionParameter] ProjectRequest project,
    [ActionParameter] LanguageRequest input)
    {
        if (!int.TryParse(project.ProjectId, out var intProjectId))
            throw new PluginMisconfigurationException($"Invalid Project ID: {project.ProjectId} must be numeric.");

        var languageId = input.LanguageId;
        if (string.IsNullOrWhiteSpace(languageId))
            throw new PluginMisconfigurationException("Language ID cannot be empty.");

        var endpoint = $"/projects/{intProjectId}/languages/{input.LanguageId}/progress";

        var enterpriseRestClient = new CrowdinEnterpriseRestClient(invocationContext.AuthenticationCredentialsProviders);

        var request = new CrowdinRestRequest(
            endpoint,
            Method.Get,
            invocationContext.AuthenticationCredentialsProviders);

        var response = await enterpriseRestClient.ExecuteWithErrorHandling(request);

        var dto = JsonConvert.DeserializeObject<Apps.Crowdin.Models.Response.Translation.LanguageProgressResponseDto>(response.Content);

        return dto;
    }
}