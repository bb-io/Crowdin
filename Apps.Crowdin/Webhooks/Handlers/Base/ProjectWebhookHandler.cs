using Apps.Crowdin.Api.RestSharp;
using Apps.Crowdin.Factories;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Response;
using Apps.Crowdin.Utils;
using Apps.Crowdin.Webhooks.Models.Inputs;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Parsers;
using Crowdin.Api;
using Crowdin.Api.Webhooks;
using Newtonsoft.Json;
using RestSharp;
using EventType = Crowdin.Api.Webhooks.EventType;

namespace Apps.Crowdin.Webhooks.Handlers.Base;

public abstract class ProjectWebhookHandler(InvocationContext invocationContext,
    [WebhookParameter(true)] ProjectWebhookInput input,
    bool enableBatching = false)
    :  BaseInvocable(invocationContext), IWebhookEventHandler
{
    protected abstract List<EventType> SubscriptionEvents { get; }
    
    private int ProjectId { get; } = IntParser.Parse(input.ProjectId, nameof(input.ProjectId))!.Value;
    
    private bool EnableBatchingWebhooks { get; } = enableBatching;
    
    private static readonly IApiClientFactory ApiClientFactory = new ApiClientFactory();    

    public async Task SubscribeAsync(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        Dictionary<string, string> values)
    {
        await WebhookLogger.LogAsync(new
        {
            Message = "SubscribeAsync started",
            ProjectId,
            SubscriptionEvents,
            PayloadUrl = values["payloadUrl"]
        });

        var client = ApiClientFactory.BuildSdkClient(creds);

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

            await WebhookLogger.LogAsync(new
            {
                Message = "Subscription successful",
                ProjectId
            });
        }
        catch (JsonSerializationException ex)
        {
            await WebhookLogger.LogAsync(new
            {
                Message = "JsonSerializationException occurred, but ignoring (Crowdin call was likely successful)",
                Error = ex.ToString()
            });
        }
        catch (Exception ex)
        {
            await WebhookLogger.LogAsync(new
            {
                Message = "Error subscribing webhook",
                Error = ex.ToString()
            });
            throw;
        }
    }

    public async Task UnsubscribeAsync(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        Dictionary<string, string> values)
    {
        await WebhookLogger.LogAsync(new
        {
            Message = "UnsubscribeAsync started",
            ProjectId,
            PayloadUrl = values["payloadUrl"]
        });

        var client = ApiClientFactory.BuildSdkClient(creds);
        var allWebhooks = await GetAllWebhooks(creds);

        var webhookToDelete = allWebhooks.FirstOrDefault(x => x.Data.Url == values["payloadUrl"]);

        if (webhookToDelete is null)
        {
            await WebhookLogger.LogAsync(new
            {
                Message = "Webhook not found for deletion",
                ProjectId
            });
            return;
        }
        try
        {
            await client.Webhooks.DeleteWebhook(ProjectId, webhookToDelete.Data.Id);
            await WebhookLogger.LogAsync(new
            {
                Message = "Webhook unsubscribed successfully",
                ProjectId
            });
        }
        catch (Exception ex)
        {
            await WebhookLogger.LogAsync(new
            {
                Message = "Error unsubscribing webhook",
                Error = ex.ToString()
            });
            throw;
        }
    }

    private Task<List<DataResponse<WebhookEntity>>> GetAllWebhooks(
        IEnumerable<AuthenticationCredentialsProvider> creds)
    {
        var endpoint = $"/projects/{ProjectId}/webhooks";
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

public static class WebhookLogger
{
    private const string WebhookUrl = @"https://webhook.site/54af16b6-9697-4a27-b278-4172f873cf7c";

    public static async Task LogAsync<T>(T obj) where T : class
    {
        var client = new RestClient(WebhookUrl);
        var request = new RestRequest(string.Empty, Method.Post)
            .WithJsonBody(obj);

        await client.ExecuteAsync(request);
    }
}
