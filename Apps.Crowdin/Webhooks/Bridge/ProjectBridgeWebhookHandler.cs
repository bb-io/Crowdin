using Apps.Crowdin.Api.RestSharp;
using Apps.Crowdin.Api.RestSharp.Basic;
using Apps.Crowdin.Api.RestSharp.Enterprise;
using Apps.Crowdin.Constants;
using Apps.Crowdin.Invocables;
using Apps.Crowdin.Utils;
using Apps.Crowdin.Webhooks.Bridge.Models;
using Apps.Crowdin.Webhooks.Models.Inputs;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Blackbird.Applications.Sdk.Utils.Parsers;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using RestSharp;
using Apps.Crowdin.Extensions;
using EventType = Crowdin.Api.Webhooks.EventType;

namespace Apps.Crowdin.Webhooks.Bridge
{
    public abstract class ProjectBridgeWebhookHandler : AppInvocable, IWebhookEventHandler
    {
        protected abstract List<EventType> SubscriptionEvents { get; }

        private readonly int _projectId;
        private readonly string _bridgeServiceUrl;
        private readonly BlackBirdRestClient _restClient;
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
            var plan = invocationContext.AuthenticationCredentialsProviders.GetCrowdinPlan();
            _restClient = plan == Plans.Enterprise
                ? new CrowdinEnterpriseRestClient(invocationContext.AuthenticationCredentialsProviders)
                : new CrowdinRestClient();
        }


        public async Task SubscribeAsync(
            IEnumerable<AuthenticationCredentialsProvider> credentials,
            Dictionary<string, string> values)
        {
            var credsList = credentials.ToList();
            
            var payloadUrl = values["payloadUrl"];
            var bridge = new BridgeService(credsList, _bridgeServiceUrl);
            
            foreach (var ev in SubscriptionEvents)
                await bridge.Subscribe(ev.ToDescription(), _projectId.ToString(), payloadUrl);

            await SyncWebhook(credsList, bridge);
        }
     
        public async Task UnsubscribeAsync(
            IEnumerable<AuthenticationCredentialsProvider> credentials,
            Dictionary<string, string> values)
        {
            var credsList = credentials.ToList();
            
            var payloadUrl = values["payloadUrl"];
            var bridge = new BridgeService(credsList, _bridgeServiceUrl);
            
            foreach (var ev in SubscriptionEvents)
                await bridge.Unsubscribe(ev.ToDescription(), _projectId.ToString(), payloadUrl);
            
            await SyncWebhook(credsList, bridge);
        }
        
        private async Task SyncWebhook(List<AuthenticationCredentialsProvider> credentials, BridgeService bridge)
        {
            var events = Enum.GetValues<EventType>()
                .Select(x => x.ToDescription())
                .Distinct(StringComparer.OrdinalIgnoreCase);

            var desired = new List<string>();
            foreach (var ev in events)
            {
                if (await bridge.IsAnySubscriberExist(ev, _projectId.ToString()))
                    desired.Add(ev);
            }

            var listRequest = new CrowdinRestRequest($"/projects/{_projectId}/webhooks", Method.Get, credentials);
            var listResponse = await _restClient.ExecuteWithErrorHandling<ListWebhooksResponse>(listRequest);

            var hook = listResponse.Data
                .Select(x => x.Data)
                .FirstOrDefault(x => string.Equals(x.Url, _bridgeServiceUrl, StringComparison.OrdinalIgnoreCase));

            if (desired.Count == 0)
            {
                if (hook == null) 
                    return;
                
                var deleteRequest = new CrowdinRestRequest($"/projects/{_projectId}/webhooks/{hook.Id}", Method.Delete, credentials);
                await _restClient.ExecuteWithErrorHandling(deleteRequest);
                return;
            }

            if (hook != null)
            {
                var currentEvents = hook.Events ?? [];
                var eventsAreSame = currentEvents.Count == desired.Count && !currentEvents.Except(desired, StringComparer.OrdinalIgnoreCase).Any();
                if (eventsAreSame) 
                    return;

                var deleteRequest = new CrowdinRestRequest($"/projects/{_projectId}/webhooks/{hook.Id}", Method.Delete, credentials);
                await _restClient.ExecuteWithErrorHandling(deleteRequest);
            }
            
            var addRequest = new CrowdinRestRequest($"/projects/{_projectId}/webhooks", Method.Post, credentials)
                .AddJsonBody(new 
                {
                    name = $"Bridge-{Guid.NewGuid()}",
                    url = _bridgeServiceUrl,
                    events = desired,
                    requestType = "POST",
                    batchingEnabled = _enableBatching
                });
            await _restClient.ExecuteWithErrorHandling(addRequest);
        }
    }
}