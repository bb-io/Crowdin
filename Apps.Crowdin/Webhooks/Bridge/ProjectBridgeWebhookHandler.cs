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
                bridge.Subscribe(ev.ToDescription(), _projectId.ToString(), payloadUrl);

            WebhookLogger.Log("bridge subscribed");
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
                bridge.Unsubscribe(ev.ToDescription(), _projectId.ToString(), payloadUrl);
            
            WebhookLogger.Log("bridge UNsubscribed");
            await SyncWebhook(credsList, bridge);
        }
        
        private async Task SyncWebhook(List<AuthenticationCredentialsProvider> credentials, BridgeService bridge)
        {
            var desired = Enum.GetValues<EventType>()
                .Select(x => x.ToDescription())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Where(x => bridge.IsAnySubscriberExist(x, _projectId.ToString()))
                .ToList();
            WebhookLogger.Log($"desired: {desired.Select(x => x.ToString())}");

            var listRequest = new CrowdinRestRequest($"/projects/{_projectId}/webhooks", Method.Get, credentials);
            var listResponse = await _restClient.ExecuteWithErrorHandling<ListWebhooksResponse>(listRequest);
            
            var hook = listResponse.Data
                .Select(x => x.Data)
                .FirstOrDefault(x => string.Equals(x.Url, _bridgeServiceUrl, StringComparison.OrdinalIgnoreCase));
            WebhookLogger.Log(hook);

            if (desired.Count == 0)
            {
                WebhookLogger.Log("0 desired");
                if (hook == null) 
                    return;
                
                var deleteRequest = new CrowdinRestRequest($"/projects/{_projectId}/webhooks/{hook.Id}", Method.Delete, credentials);
                await _restClient.ExecuteWithErrorHandling(deleteRequest);
                WebhookLogger.Log("deleted");
                return;
            }

            if (hook == null)
            {
                WebhookLogger.Log("hook is null");
                var addReq = new CrowdinRestRequest($"/projects/{_projectId}/webhooks", Method.Post, credentials);
                addReq.AddJsonBody(new
                {
                    name = $"Bridge-{Guid.NewGuid()}",
                    url = _bridgeServiceUrl,
                    events = desired,
                    requestType = "POST",
                    batchingEnabled = _enableBatching
                });
                await _restClient.ExecuteWithErrorHandling(addReq);
                WebhookLogger.Log("created");
                return;
            }

            var currentEvents = hook.Events ?? [];
            bool eventsAreSame = currentEvents.Count == desired.Count && !currentEvents.Except(desired, StringComparer.OrdinalIgnoreCase).Any();
            WebhookLogger.Log($"current events: {currentEvents.Select(x => x.ToString())}, eventsAreSame: {eventsAreSame}");
            if (!eventsAreSame)
            {
                WebhookLogger.Log("events are not same");
                var patchRequest = new CrowdinRestRequest($"/projects/{_projectId}/webhooks/{hook.Id}", Method.Patch, credentials)
                    .AddJsonBody(new[] { new { op = "replace", path = "/events", value = desired } });
                
                await _restClient.ExecuteWithErrorHandling(patchRequest);
            }
        }
    }
}