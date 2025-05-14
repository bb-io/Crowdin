using Apps.Crowdin.Api.RestSharp;
using Apps.Crowdin.Api.RestSharp.Basic;
using Apps.Crowdin.Webhooks.Bridge.Models;
using Apps.Crowdin.Webhooks.Models.Inputs;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Blackbird.Applications.Sdk.Utils.Parsers;
using Crowdin.Api.Webhooks;
using Microsoft.Extensions.Logging;
using RestSharp;
using System.ComponentModel;
using System.Text.Json;
using EventType = Crowdin.Api.Webhooks.EventType;

namespace Apps.Crowdin.Webhooks.Bridge
{
    public abstract class ProjectBridgeWebhookHandler : BaseInvocable, IWebhookEventHandler
    {
        protected abstract List<EventType> SubscriptionEvents { get; }

        private readonly int _projectId;
        private readonly string _bridgeServiceUrl;
        private readonly CrowdinRestClient _restClient;
        private readonly bool _enableBatching;

        protected ProjectBridgeWebhookHandler(
            InvocationContext invocationContext,
            [WebhookParameter(true)] ProjectWebhookInput input,
            bool enableBatching = false)
            : base(invocationContext)
        {
            _projectId = IntParser.Parse(input.ProjectId, nameof(input.ProjectId))!.Value;
            _enableBatching = enableBatching;
            _bridgeServiceUrl = $"{invocationContext.UriInfo.BridgeServiceUrl.ToString().TrimEnd('/')}/webhooks/crowdin";
            _restClient = new CrowdinRestClient();
        }


        public async Task SubscribeAsync(
            IEnumerable<AuthenticationCredentialsProvider> credentials,
            Dictionary<string, string> values)
        {
            var payloadUrl = values["payloadUrl"];
            var bridge = new BridgeService(credentials, _bridgeServiceUrl);
            foreach (var ev in SubscriptionEvents)
            {
                bridge.Subscribe(ev.ToDescription(), _projectId.ToString(), payloadUrl);
            }

            var listReq = new CrowdinRestRequest($"/projects/{_projectId}/webhooks", Method.Get, credentials);
            var listResp = await _restClient.ExecuteAsync<ListWebhooksResponse>(listReq);
            var exists = listResp.Data?.Webhooks.Any(w =>
                string.Equals(w.Url, _bridgeServiceUrl, StringComparison.OrdinalIgnoreCase)) ?? false;

            if (exists)
            { 
                return;
            }    
                

            var addReq = new CrowdinRestRequest($"/projects/{_projectId}/webhooks", Method.Post, credentials);
            var eventStrings = SubscriptionEvents.Select(e => e.ToDescription()).ToList();

            var webhookRequest = new
            {
                name = $"Bridge-{Guid.NewGuid()}",
                url = _bridgeServiceUrl,
                events = SubscriptionEvents.Select(e => e.ToDescription()).ToList(),
                requestType = "POST",
                batchingEnabled = _enableBatching
            };

            addReq.AddJsonBody(webhookRequest);

            var addResp = await _restClient.ExecuteAsync(addReq);
            var rawResponseJson = addResp.Content ?? "No response content";

            if (!addResp.IsSuccessful)
            {   
                throw new Exception($"Failed to create Crowdin webhook. Status: {addResp.StatusCode}, Error: {addResp.ErrorMessage}");
            }
        }
     
        public async Task UnsubscribeAsync(
            IEnumerable<AuthenticationCredentialsProvider> credentials,
            Dictionary<string, string> values)
        {
            var payloadUrl = values["payloadUrl"];
            var bridge = new BridgeService(credentials, _bridgeServiceUrl);
            foreach (var ev in SubscriptionEvents)
            {
                bridge.Unsubscribe(ev.ToDescription(), _projectId.ToString(), payloadUrl);
            }

            var listReq = new CrowdinRestRequest($"/projects/{_projectId}/webhooks", Method.Get, credentials);
            var listResp = await _restClient.ExecuteAsync<ListWebhooksResponse>(listReq);
            var toDelete = listResp.Data?.Webhooks
                .Where(w => string.Equals(w.Url, _bridgeServiceUrl, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (toDelete is null) return;

            foreach (var hook in toDelete)
            {
                var delReq = new CrowdinRestRequest($"/projects/{_projectId}/webhooks/{hook.Id}", Method.Delete, credentials);
                await _restClient.ExecuteAsync(delReq);
            }
        }
    }

    public static class EnumExtensions
    {
        public static string ToDescription(this Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            if (fi == null) return value.ToString();

            var attr = fi.GetCustomAttributes(typeof(DescriptionAttribute), false)
                         .OfType<DescriptionAttribute>()
                         .FirstOrDefault();
            return attr?.Description ?? value.ToString();
        }
    }
}

