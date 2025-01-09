using Apps.Crowdin.Webhooks.Handlers.Organization;
using Apps.Crowdin.Webhooks.Handlers.Project.Groups;
using Apps.Crowdin.Webhooks.Models.Payload;
using Apps.Crowdin.Webhooks.Models.Payload.Project.Response;
using Apps.Crowdin.Webhooks.Models.Payload.Project.Wrappers;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;

namespace Apps.Crowdin.Webhooks.Lists;

[WebhookList]
public class OrganizationWebhookList
{
    [Webhook("On project created", typeof(ProjectCreatedHandler), Description = "On project created")]
    public Task<WebhookResponse<ProjectWithUserWebhookResponse>> OnProjectCreated(WebhookRequest webhookRequest)
        => OnProjectAction(webhookRequest);

    [Webhook("On project deleted", typeof(ProjectDeletedHandler), Description = "On project deleted")]
    public Task<WebhookResponse<ProjectWithUserWebhookResponse>> OnProjectDeleted(WebhookRequest webhookRequest)
        => OnProjectAction(webhookRequest);
    
    [Webhook("[Enterprise] On group created", typeof(GroupCreatedHandler), Description = "On group created")]
    public Task<WebhookResponse<GroupWebhookResponse>> OnGroupCreated(WebhookRequest webhookRequest)
        => HandleWebhookRequest<GroupWebhookResponse, GroupWrapper>(webhookRequest);

    [Webhook("[Enterprise] On group deleted", typeof(GroupDeletedHandler), Description = "On group deleted")]
    public Task<WebhookResponse<GroupWebhookResponse>> OnGroupDeleted(WebhookRequest webhookRequest)
        => HandleWebhookRequest<GroupWebhookResponse, GroupWrapper>(webhookRequest);
    
    private Task<WebhookResponse<T>> HandleWebhookRequest<T, TV>(WebhookRequest webhookRequest) where T : CrowdinWebhookResponse<TV>, new()
    {
        var data = JsonConvert.DeserializeObject<TV>(webhookRequest.Body.ToString());

        if (data is null)
            throw new InvalidCastException(nameof(webhookRequest.Body));

        var result = new T();
        result.ConfigureResponse(data);

        return Task.FromResult(new WebhookResponse<T>
        {
            Result = result
        });
    }

    public Task<WebhookResponse<ProjectWithUserWebhookResponse>> OnProjectAction(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<ProjectWithUserWrapper>(webhookRequest.Body.ToString());

        if (data is null)
            throw new InvalidCastException(nameof(webhookRequest.Body));

        var result = new ProjectWithUserWebhookResponse();
        result.ConfigureResponse(data);
        
        
        return Task.FromResult(new WebhookResponse<ProjectWithUserWebhookResponse>
        {
            Result = result
        });
    }
}