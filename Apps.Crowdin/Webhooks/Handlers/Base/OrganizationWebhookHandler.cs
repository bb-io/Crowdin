using Apps.Crowdin.Api;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Crowdin.Api.Webhooks;
using Crowdin.Api.Webhooks.Organization;
using AddWebhookRequest = Crowdin.Api.Webhooks.Organization.AddWebhookRequest;

namespace Apps.Crowdin.Webhooks.Handlers.Base;

public abstract class OrganizationWebhookHandler : IWebhookEventHandler
{
    protected abstract OrganizationEventType SubscriptionEvent { get; }

    public Task SubscribeAsync(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        Dictionary<string, string> values)
    {
        var client = new CrowdinClient(creds);
        var executor = new OrganizationWebhooksApiExecutor(client);

        var request = new AddWebhookRequest
        {
            Name = $"{SubscriptionEvent}-{Guid.NewGuid()}",
            Url = values["payloadUrl"],
            RequestType = RequestType.POST,
            Events = new List<OrganizationEventType> { SubscriptionEvent }
        };

        return executor.AddWebhook(request);
    }

    public async Task UnsubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> creds,
        Dictionary<string, string> values)
    {
        var client = new CrowdinClient(creds);
        var executor = new OrganizationWebhooksApiExecutor(client);

        var allWebhooks = await GetAllWebhooks(executor);
        var webhookToDelete = allWebhooks.First(x => x.Url == values["payloadUrl"]);
        
        await executor.DeleteWebhook(webhookToDelete.Id);
    }

    private Task<List<OrganizationWebhookResource>> GetAllWebhooks(OrganizationWebhooksApiExecutor client)
        => Paginator.Paginate(client.ListWebhooks);
}