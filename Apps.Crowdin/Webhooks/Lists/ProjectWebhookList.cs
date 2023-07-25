using Apps.Crowdin.Webhooks.Handlers.Project.File;
using Apps.Crowdin.Webhooks.Handlers.Project.Project;
using Apps.Crowdin.Webhooks.Handlers.Project.Translation;
using Apps.Crowdin.Webhooks.Models.Payload.File.Response;
using Apps.Crowdin.Webhooks.Models.Payload.File.Wrappers;
using Apps.Crowdin.Webhooks.Models.Payload.Project.Response;
using Apps.Crowdin.Webhooks.Models.Payload.Project.Wrappers;
using Apps.Crowdin.Webhooks.Models.Payload.Translation.Response;
using Apps.Crowdin.Webhooks.Models.Payload.Translation.Wrappers;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;

namespace Apps.Crowdin.Webhooks.Lists;

[WebhookList]
public class ProjectWebhookList
{
    #region File

    [Webhook("On file added", typeof(FileAddedHandler), Description = "On file added")]
    public Task<WebhookResponse<FileWithUserWebhookResponse>> OnFileAdded(WebhookRequest webhookRequest)
        => OnFileWithUser(webhookRequest);

    [Webhook("On file approved", typeof(FileApprovedHandler), Description = "On file approved")]
    public Task<WebhookResponse<FileWithLanguageWebhookResponse>> OnFileApproved(WebhookRequest webhookRequest)
        => OnFileWithLanguage(webhookRequest);
    
    [Webhook("On file deleted", typeof(FileDeletedHandler), Description = "On file deleted")]
    public Task<WebhookResponse<FileWithUserWebhookResponse>> OnFileDeleted(WebhookRequest webhookRequest)
        => OnFileWithUser(webhookRequest);    
    
    [Webhook("On file reverted", typeof(FileRevertedHandler), Description = "On file reverted")]
    public Task<WebhookResponse<FileWithUserWebhookResponse>> OnFileReverted(WebhookRequest webhookRequest)
        => OnFileWithUser(webhookRequest);  
    
    [Webhook("On file translated", typeof(FileTranslatedHandler), Description = "On file translated")]
    public Task<WebhookResponse<FileWithLanguageWebhookResponse>> OnFileTranslated(WebhookRequest webhookRequest)
        => OnFileWithLanguage(webhookRequest);
    
    [Webhook("On file updated", typeof(FileUpdatedHandler), Description = "On file updated")]
    public Task<WebhookResponse<FileWithUserWebhookResponse>> OnFileUpdated(WebhookRequest webhookRequest)
        => OnFileWithUser(webhookRequest);

    public Task<WebhookResponse<FileWithUserWebhookResponse>> OnFileWithUser(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<FileWithUserWrapper>(webhookRequest.Body.ToString());

        if (data is null)
            throw new InvalidCastException(nameof(webhookRequest.Body));

        return Task.FromResult(new WebhookResponse<FileWithUserWebhookResponse>
        {
            Result = new(data)
        });
    }
    
    public Task<WebhookResponse<FileWithLanguageWebhookResponse>> OnFileWithLanguage(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<FileWithLanguageWrapper>(webhookRequest.Body.ToString());

        if (data is null)
            throw new InvalidCastException(nameof(webhookRequest.Body));

        return Task.FromResult(new WebhookResponse<FileWithLanguageWebhookResponse>
        {
            Result = new(data)
        });
    }

    #endregion

    #region Project

    [Webhook("On project approved", typeof(ProjectApprovedHandler), Description = "On project approved")]
    public Task<WebhookResponse<ProjectWithLanguageWebhookResponse>> OnProjectApproved(WebhookRequest webhookRequest)
        => OnProjectWithLanguage(webhookRequest);
    
    [Webhook("On project translated", typeof(ProjectTranslatedHandler), Description = "On project translated")]
    public Task<WebhookResponse<ProjectWithLanguageWebhookResponse>> OnProjectTranslated(WebhookRequest webhookRequest)
        => OnProjectWithLanguage(webhookRequest);

    [Webhook("On project built", typeof(ProjectBuiltHandler), Description = "On project built")]
    public Task<WebhookResponse<ProjectBuiltWebhookResponse>> OnProjectBuilt(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<ProjectBuildWrapper>(webhookRequest.Body.ToString());

        if (data is null)
            throw new InvalidCastException(nameof(webhookRequest.Body));

        return Task.FromResult(new WebhookResponse<ProjectBuiltWebhookResponse>
        {
            Result = new(data)
        });
    }

    public Task<WebhookResponse<ProjectWithLanguageWebhookResponse>> OnProjectWithLanguage(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<ProjectWithLanguageWrapper>(webhookRequest.Body.ToString());

        if (data is null)
            throw new InvalidCastException(nameof(webhookRequest.Body));

        return Task.FromResult(new WebhookResponse<ProjectWithLanguageWebhookResponse>
        {
            Result = new(data)
        });
    }
    
    #endregion

    #region String

    #endregion

    #region StringComment

    #endregion

    #region Suggestion

    #endregion

    #region Task

    #endregion

    #region Translation

    [Webhook("On translation updated", typeof(TranslationUpdatedHandler), Description = "On translation updated")]
    public Task<WebhookResponse<TranslationUpdatedWebhookResponse>> OnTranslationUpdated(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<TranslationUpdatedWrapper>(webhookRequest.Body.ToString());

        if (data is null)
            throw new InvalidCastException(nameof(webhookRequest.Body));

        return Task.FromResult(new WebhookResponse<TranslationUpdatedWebhookResponse>
        {
            Result = new(data)
        });
    }

    #endregion
}