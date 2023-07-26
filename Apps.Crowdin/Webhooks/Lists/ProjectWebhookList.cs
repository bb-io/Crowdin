using Apps.Crowdin.Webhooks.Handlers.Project.File;
using Apps.Crowdin.Webhooks.Handlers.Project.Project;
using Apps.Crowdin.Webhooks.Handlers.Project.String;
using Apps.Crowdin.Webhooks.Handlers.Project.StringComment;
using Apps.Crowdin.Webhooks.Handlers.Project.Suggestion;
using Apps.Crowdin.Webhooks.Handlers.Project.Task;
using Apps.Crowdin.Webhooks.Handlers.Project.Translation;
using Apps.Crowdin.Webhooks.Models;
using Apps.Crowdin.Webhooks.Models.Payload;
using Apps.Crowdin.Webhooks.Models.Payload.File.Response;
using Apps.Crowdin.Webhooks.Models.Payload.File.Wrappers;
using Apps.Crowdin.Webhooks.Models.Payload.Project.Response;
using Apps.Crowdin.Webhooks.Models.Payload.Project.Wrappers;
using Apps.Crowdin.Webhooks.Models.Payload.String;
using Apps.Crowdin.Webhooks.Models.Payload.String.Response;
using Apps.Crowdin.Webhooks.Models.Payload.StringComment.Response;
using Apps.Crowdin.Webhooks.Models.Payload.StringComment.Wrappers;
using Apps.Crowdin.Webhooks.Models.Payload.Suggestion.Response;
using Apps.Crowdin.Webhooks.Models.Payload.Suggestion.Wrappers;
using Apps.Crowdin.Webhooks.Models.Payload.Task.Response;
using Apps.Crowdin.Webhooks.Models.Payload.Task.Wrapper;
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
    public Task<WebhookResponse<SimpleFileWebhookResponse>> OnFileAdded(WebhookRequest webhookRequest)
        => HandleWehookRequest<SimpleUserFileWrapper, SimpleFileWebhookResponse>(webhookRequest);

    [Webhook("On file approved", typeof(FileApprovedHandler), Description = "On file approved")]
    public Task<WebhookResponse<SimpleLanguageFileWebhookResponse>> OnFileApproved(WebhookRequest webhookRequest)
        => HandleWehookRequest<SimpleLanguageFileWrapper, SimpleLanguageFileWebhookResponse>(webhookRequest);

    [Webhook("On file deleted", typeof(FileDeletedHandler), Description = "On file deleted")]
    public Task<WebhookResponse<SimpleFileWebhookResponse>> OnFileDeleted(WebhookRequest webhookRequest)
        => HandleWehookRequest<SimpleUserFileWrapper, SimpleFileWebhookResponse>(webhookRequest);

    [Webhook("On file reverted", typeof(FileRevertedHandler), Description = "On file reverted")]
    public Task<WebhookResponse<SimpleFileWebhookResponse>> OnFileReverted(WebhookRequest webhookRequest)
        => HandleWehookRequest<SimpleUserFileWrapper, SimpleFileWebhookResponse>(webhookRequest);

    [Webhook("On file translated", typeof(FileTranslatedHandler), Description = "On file fully translated")]
    public Task<WebhookResponse<SimpleLanguageFileWebhookResponse>> OnFileTranslated(WebhookRequest webhookRequest)
        => HandleWehookRequest<SimpleLanguageFileWrapper, SimpleLanguageFileWebhookResponse>(webhookRequest);

    [Webhook("On file updated", typeof(FileUpdatedHandler), Description = "On file updated")]
    public Task<WebhookResponse<SimpleFileWebhookResponse>> OnFileUpdated(WebhookRequest webhookRequest)
        => HandleWehookRequest<SimpleUserFileWrapper, SimpleFileWebhookResponse>(webhookRequest);

    #endregion

    #region Project

    [Webhook("On project approved", typeof(ProjectApprovedHandler), Description = "On project approved")]
    public Task<WebhookResponse<SimpleProjectResponse>> OnProjectApproved(WebhookRequest webhookRequest)
        => HandleWehookRequest<SimpleProjectWrapper, SimpleProjectResponse>(webhookRequest);

    [Webhook("On project translated", typeof(ProjectTranslatedHandler), Description = "On project translated")]
    public Task<WebhookResponse<SimpleProjectResponse>> OnProjectTranslated(WebhookRequest webhookRequest)
        => HandleWehookRequest<SimpleProjectWrapper, SimpleProjectResponse>(webhookRequest);

    [Webhook("On project built", typeof(ProjectBuiltHandler), Description = "On project built")]
    public Task<WebhookResponse<SimpleProjectBuiltWebhookResponse>> OnProjectBuilt(WebhookRequest webhookRequest)
        => HandleWehookRequest<SimpleProjectBuildWrapper, SimpleProjectBuiltWebhookResponse>(webhookRequest);

    #endregion

    #region String

    [Webhook("On string added", typeof(StringAddedHandler), Description = "On string added")]
    public Task<WebhookResponse<SimpleStringWebhookResponse>> OnStringAdded(WebhookRequest webhookRequest)
        => HandleWehookRequest<EventsWebhookResponse<SimpleStringWrapper>, SimpleStringWebhookResponse>(webhookRequest);

    [Webhook("On string deleted", typeof(StringDeletedHandler), Description = "On string deleted")]
    public Task<WebhookResponse<SimpleStringWebhookResponse>> OnStringDeleted(WebhookRequest webhookRequest)
        => HandleWehookRequest<EventsWebhookResponse<SimpleStringWrapper>, SimpleStringWebhookResponse>(webhookRequest);

    [Webhook("On string updated", typeof(StringUpdatedHandler), Description = "On string updated")]
    public Task<WebhookResponse<SimpleStringWebhookResponse>> OnStringUpdated(WebhookRequest webhookRequest)
        => HandleWehookRequest<EventsWebhookResponse<SimpleStringWrapper>, SimpleStringWebhookResponse>(webhookRequest);

    #endregion

    #region StringComment

    [Webhook("On string comment created", typeof(StringCommentCreatedHandler), Description = "On string comment created")]
    public Task<WebhookResponse<StringCommentWebhookResponse>> OnStringCommentCreated(WebhookRequest webhookRequest)
        => HandleWehookRequest<StringCommentWrapper, StringCommentWebhookResponse>(webhookRequest);
    
    [Webhook("On string comment deleted", typeof(StringCommentDeletedHandler), Description = "On string comment deleted")]
    public Task<WebhookResponse<StringCommentWebhookResponse>> OnStringCommentDeleted(WebhookRequest webhookRequest)
        => HandleWehookRequest<StringCommentWrapper, StringCommentWebhookResponse>(webhookRequest);    
    
    [Webhook("On string comment restored", typeof(StringCommentRestoredHandler), Description = "On string comment restored")]
    public Task<WebhookResponse<StringCommentWebhookResponse>> OnStringCommentRestored(WebhookRequest webhookRequest)
        => HandleWehookRequest<StringCommentWrapper, StringCommentWebhookResponse>(webhookRequest);
    
    [Webhook("On string comment updated", typeof(StringCommentUpdatedHandler), Description = "On string comment updated")]
    public Task<WebhookResponse<StringCommentWebhookResponse>> OnStringCommentUpdated(WebhookRequest webhookRequest)
        => HandleWehookRequest<StringCommentWrapper, StringCommentWebhookResponse>(webhookRequest);

    #endregion

    #region Suggestion

    [Webhook("On suggestion added", typeof(SuggestionAddedHandler), Description = "On suggestion added")]
    public Task<WebhookResponse<SimpleSuggestionWebhookResponse>> OnSuggestionAdded(WebhookRequest webhookRequest)
        => HandleWehookRequest<SimpleSuggestionWrapper, SimpleSuggestionWebhookResponse>(webhookRequest);

    [Webhook("On suggestion approved", typeof(SuggestionApprovedHandler), Description = "On suggestion approved")]
    public Task<WebhookResponse<SimpleSuggestionWebhookResponse>> OnSuggestionApproved(WebhookRequest webhookRequest)
        => HandleWehookRequest<SimpleSuggestionWrapper, SimpleSuggestionWebhookResponse>(webhookRequest);

    [Webhook("On suggestion deleted", typeof(SuggestionDeletedHandler), Description = "On suggestion deleted")]
    public Task<WebhookResponse<SimpleSuggestionWebhookResponse>> OnSuggestionDeleted(WebhookRequest webhookRequest)
        => HandleWehookRequest<SimpleSuggestionWrapper, SimpleSuggestionWebhookResponse>(webhookRequest);

    [Webhook("On suggestion disapproved", typeof(SuggestionDisapprovedHandler), Description = "On suggestion disapproved")]
    public Task<WebhookResponse<SimpleSuggestionWebhookResponse>> OnSuggestionDisapproved(WebhookRequest webhookRequest)
        => HandleWehookRequest<SimpleSuggestionWrapper, SimpleSuggestionWebhookResponse>(webhookRequest);

    [Webhook("On suggestion updated", typeof(SuggestionUpdatedHandler), Description = "On suggestion updated")]
    public Task<WebhookResponse<SimpleSuggestionWebhookResponse>> OnSuggestionUpdated(WebhookRequest webhookRequest)
        => HandleWehookRequest<SimpleSuggestionWrapper, SimpleSuggestionWebhookResponse>(webhookRequest);

    #endregion

    #region Task

    [Webhook("On task added", typeof(TaskAddedHandler), Description = "On task added")]
    public Task<WebhookResponse<SimpleTaskWebhookResponse>> OnTaskAdded(WebhookRequest webhookRequest)
        => HandleWehookRequest<SimpleTaskWrapper, SimpleTaskWebhookResponse>(webhookRequest);
    
    [Webhook("On task deleted", typeof(TaskDeletedHandler), Description = "On task deleted")]
    public Task<WebhookResponse<SimpleTaskWebhookResponse>> OnTaskDeleted(WebhookRequest webhookRequest)
        => HandleWehookRequest<SimpleTaskWrapper, SimpleTaskWebhookResponse>(webhookRequest);
    
    [Webhook("On task status changed", typeof(TaskStatusChangedHandler), Description = "On task status changed")]
    public Task<WebhookResponse<SimpleTaskStatusChangedWebhookResponse>> OnTaskStatusChanged(WebhookRequest webhookRequest)
        => HandleWehookRequest<SimpleTaskStatusChangedWrapper, SimpleTaskStatusChangedWebhookResponse>(webhookRequest);
    
    #endregion

    #region Translation

    [Webhook("On translation updated", typeof(TranslationUpdatedHandler), Description = "On translation updated")]
    public Task<WebhookResponse<SimpleTranslationWebhookResponse>> OnTranslationUpdated(WebhookRequest webhookRequest)
        => HandleWehookRequest<SimpleTranslationWrapper, SimpleTranslationWebhookResponse>(webhookRequest);

    #endregion

    public async Task<WebhookResponse<TV>> HandleWehookRequest<T, TV>(WebhookRequest webhookRequest)
        where TV : CrowdinWebhookResponse<T>, new()
    {
        await Logger.Log(webhookRequest);
        var data = JsonConvert.DeserializeObject<T>(webhookRequest.Body.ToString());
        await Logger.Log(data);

        if (data is null)
            throw new InvalidCastException(nameof(webhookRequest.Body));

        var result = new TV();
        result.ConfigureResponse(data);

        await Logger.Log(result);

        return new WebhookResponse<TV>
        {
            Result = result
        };
    }
}