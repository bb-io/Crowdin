using Apps.Crowdin.Webhooks.Handlers.Organization;
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

    public Task<WebhookResponse<ProjectWithUserWebhookResponse>> OnProjectAction(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<ProjectWithUserWrapper>(webhookRequest.Body.ToString());

        if (data is null)
            throw new InvalidCastException(nameof(webhookRequest.Body));

        return Task.FromResult(new WebhookResponse<ProjectWithUserWebhookResponse>
        {
            Result = new(data)
        });
    }
}