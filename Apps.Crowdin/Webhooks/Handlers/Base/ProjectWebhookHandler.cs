using Apps.Crowdin.Api;
using Apps.Crowdin.Api.RestSharp;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Response;
using Apps.Crowdin.Utils;
using Apps.Crowdin.Utils.Parsers;
using Apps.Crowdin.Webhooks.Models.Inputs;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Crowdin.Api;
using Crowdin.Api.Webhooks;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Crowdin.Webhooks.Handlers.Base;

public abstract class ProjectWebhookHandler : IWebhookEventHandler
{
    protected abstract EventType SubscriptionEvent { get; }
    private int ProjectId { get; }
    private bool EnableBatchingWebhooks { get; }

    protected ProjectWebhookHandler([WebhookParameter] ProjectWebhookInput input, bool enableBatching = false)
    {
        ProjectId = IntParser.Parse(input.ProjectId, nameof(input.ProjectId))!.Value;
        EnableBatchingWebhooks = enableBatching;
    }

    public async Task SubscribeAsync(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        Dictionary<string, string> values)
    {
        var client = new CrowdinClient(creds);

        var request = new AddWebhookRequest
        {
            Name = $"BlackBird-{SubscriptionEvent}-{Guid.NewGuid()}",
            Url = " https://webhook.site/37aecaf2-ce6b-4817-8788-79eb3a06832f",
            RequestType = RequestType.POST,
            Events = new List<EventType> { SubscriptionEvent },
            BatchingEnabled = EnableBatchingWebhooks
        };

        try
        {
            await client.Webhooks.AddWebhook(ProjectId, request);
        }
        catch (JsonSerializationException)
        {
            // SDK deserializes response wrong, but the request itself is successful
        }
    }

    public async Task UnsubscribeAsync(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        Dictionary<string, string> values)
    {
        var client = new CrowdinClient(creds);
        var allWebhooks = await GetAllWebhooks(creds);

        var webhookToDelete = allWebhooks.FirstOrDefault(x => x.Data.Url == values["payloadUrl"]);

        if (webhookToDelete is null)
            return;
        
        await client.Webhooks.DeleteWebhook(ProjectId, webhookToDelete.Data.Id);
    }

    private Task<List<DataResponse<WebhookEntity>>> GetAllWebhooks(
        IEnumerable<AuthenticationCredentialsProvider> creds)
    {
        var endpoint = $"/projects/{ProjectId}/webhooks";
        var client = new CrowdinRestClient();
        
        return Paginator.Paginate(async (lim, offset) =>
        {
            var source = $"{endpoint}?limit={lim}&offset={offset}";
            var request = new CrowdinRestRequest(source, Method.Get, creds);

            var response = await client.ExecuteAsync<ResponseList<DataResponse<WebhookEntity>>>(request);
            return response.Data;
        });
    }
}