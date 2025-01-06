using Apps.Crowdin.Api;
using Apps.Crowdin.Api.RestSharp;
using Apps.Crowdin.Api.RestSharp.Basic;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Response;
using Apps.Crowdin.Utils;
using Apps.Crowdin.Webhooks.Models.Inputs;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Blackbird.Applications.Sdk.Utils.Parsers;
using Crowdin.Api;
using Crowdin.Api.Webhooks;
using Newtonsoft.Json;
using RestSharp;
using EventType = Crowdin.Api.Webhooks.EventType;

namespace Apps.Crowdin.Webhooks.Handlers.Base;

public abstract class ProjectWebhookHandler(
    [WebhookParameter(true)] ProjectWebhookInput input,
    bool enableBatching = false)
    : IWebhookEventHandler
{
    protected abstract List<EventType> SubscriptionEvents { get; }
    private int ProjectId { get; } = IntParser.Parse(input.ProjectId, nameof(input.ProjectId))!.Value;
    private bool EnableBatchingWebhooks { get; } = enableBatching;

    public async Task SubscribeAsync(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        Dictionary<string, string> values)
    {
        var client = new CrowdinClient(creds);

        var request = new AddWebhookRequest
        {
            Name = $"BlackBird-{Guid.NewGuid()}",
            Url = values["payloadUrl"],
            RequestType = RequestType.POST,
            Events = SubscriptionEvents,
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