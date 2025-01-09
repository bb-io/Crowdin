using Apps.Crowdin.Api.RestSharp;
using Apps.Crowdin.Factories;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Response;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Crowdin.Api;
using Crowdin.Api.Webhooks;
using Crowdin.Api.Webhooks.Organization;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Crowdin.Webhooks.Handlers.Base;

public abstract class EnterpriseOrganizationWebhookHandler : IWebhookEventHandler
{
    protected abstract EnterpriseOrgEventType SubscriptionEvent { get; }
    
    private static readonly IApiClientFactory ApiClientFactory = new ApiClientFactory();    

    public async Task SubscribeAsync(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        Dictionary<string, string> values)
    {
        var client = ApiClientFactory.BuildSdkClient(creds);
        var executor = new OrganizationWebhooksApiExecutor(client);

        var request = new EnterpriseAddWebhookRequest
        {
            Name = $"BlackBird-{SubscriptionEvent}-{Guid.NewGuid()}",
            Url = values["payloadUrl"],
            RequestType = RequestType.POST,
            Events = new List<EnterpriseOrgEventType> { SubscriptionEvent }
        };

        try
        {
            await executor.AddWebhook(request);
        }
        catch (JsonSerializationException)
        {
            // SDK deserializes response wrong, but the request itself is successful
        }
    }

    public async Task UnsubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> creds,
        Dictionary<string, string> values)
    {
        var client = ApiClientFactory.BuildSdkClient(creds);
        var executor = new OrganizationWebhooksApiExecutor(client);

        var allWebhooks = await GetAllWebhooks(creds);
        var webhookToDelete = allWebhooks.FirstOrDefault(x => x.Data.Url == values["payloadUrl"]);

        if (webhookToDelete is null)
            return;
        
        await executor.DeleteWebhook(webhookToDelete.Data.Id);
    }

    private Task<List<DataResponse<WebhookEntity>>> GetAllWebhooks(
        IEnumerable<AuthenticationCredentialsProvider> creds)
    {
        var endpoint = "/webhooks";
        var client = ApiClientFactory.BuildRestClient(creds);
        
        return Paginator.Paginate(async (lim, offset) =>
        {
            var source = $"{endpoint}?limit={lim}&offset={offset}";
            var request = new CrowdinRestRequest(source, Method.Get, creds);

            var response = await client.ExecuteAsync<ResponseList<DataResponse<WebhookEntity>>>(request);
            return response.Data!;
        });
    }
}