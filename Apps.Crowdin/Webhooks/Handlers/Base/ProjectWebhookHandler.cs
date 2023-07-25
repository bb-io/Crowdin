using Apps.Crowdin.Api;
using Apps.Crowdin.Utils;
using Apps.Crowdin.Utils.Parsers;
using Apps.Crowdin.Webhooks.Models.Inputs;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Crowdin.Api.Webhooks;

namespace Apps.Crowdin.Webhooks.Handlers.Base;

public abstract class ProjectWebhookHandler : IWebhookEventHandler
{
    protected abstract EventType SubscriptionEvent { get; }
    private int ProjectId { get; }

    protected ProjectWebhookHandler([WebhookParameter] ProjectWebhookInput input)
    {
        ProjectId = IntParser.Parse(input.ProjectId, nameof(input.ProjectId))!.Value;
    }

    public Task SubscribeAsync(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        Dictionary<string, string> values)
    {
        var client = new CrowdinClient(creds);

        var request = new AddWebhookRequest
        {
            Name = $"{SubscriptionEvent}-{Guid.NewGuid()}",
            Url = values["payloadUrl"],
            RequestType = RequestType.POST,
            Events = new List<EventType> { SubscriptionEvent }
        };

        return client.Webhooks.AddWebhook(ProjectId, request);
    }

    public async Task UnsubscribeAsync(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        Dictionary<string, string> values)
    {
        var client = new CrowdinClient(creds);
        var allWebhooks = await GetAllWebhooks(client);

        var webhookToDelete = allWebhooks.First(x => x.Url == values["payloadUrl"]);
        await client.Webhooks.DeleteWebhook(ProjectId, webhookToDelete.Id);
    }

    private Task<List<Webhook>> GetAllWebhooks(CrowdinClient client)
        => Paginator.Paginate((lim, offset) 
            => client.Webhooks.ListWebhooks(ProjectId, lim, offset));
}