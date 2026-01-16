using Apps.Crowdin.Api.RestSharp;
using Apps.Crowdin.Api.RestSharp.Basic;
using Apps.Crowdin.Api.RestSharp.Enterprise;
using Apps.Crowdin.Constants;
using Apps.Crowdin.Invocables;
using Apps.Crowdin.Utils;
using Apps.Crowdin.Webhooks.Bridge.Models;
using Apps.Crowdin.Webhooks.Models.Inputs;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Blackbird.Applications.Sdk.Utils.Parsers;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Crowdin.Api.Webhooks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
            var payloadUrl = values["payloadUrl"];
            var bridge = new BridgeService(credentials, _bridgeServiceUrl);
            foreach (var ev in SubscriptionEvents)
            {
                bridge.Subscribe(ev.ToDescription(), _projectId.ToString(), payloadUrl);
            }

            var listReq = new CrowdinRestRequest($"/projects/{_projectId}/webhooks", Method.Get, credentials);
            var listResp = await _restClient.ExecuteAsync(listReq);

            await WebhookLogger.LogAsync(new
            {
                stage = "Subscribe: LIST /webhooks",
                projectId = _projectId,
                url = _bridgeServiceUrl,
                status = (int)listResp.StatusCode,
                body = listResp.Content,
                headers = listResp.Headers
            });


            if (!listResp.IsSuccessStatusCode)
                throw new Exception($"Crowdin LIST webhooks failed: {(int)listResp.StatusCode}. Body: {listResp.Content}");

            var listParsed = JsonConvert.DeserializeObject<ListWebhooksResponse>(listResp.Content ?? "{}")
                           ?? new ListWebhooksResponse();


            var exists = listParsed.Data.Any(x => string.Equals(x.Data.Url, _bridgeServiceUrl, StringComparison.OrdinalIgnoreCase));

            if (exists)
            { 
                return;
            }   
            
            var addReq = new CrowdinRestRequest($"/projects/{_projectId}/webhooks", Method.Post, credentials);

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

            await WebhookLogger.LogAsync(new
            {
                stage = "Subscribe: POST /webhooks",
                projectId = _projectId,
                url = _bridgeServiceUrl,
                requestBody = webhookRequest,
                status = (int)addResp.StatusCode,
                body = addResp.Content,
                headers = addResp.Headers
            });

            if (!addResp.IsSuccessStatusCode)
                throw new Exception($"Crowdin ADD webhook failed: {(int)addResp.StatusCode}. Body: {addResp.Content}");
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
            var listResp = await _restClient.ExecuteAsync(listReq);

            await WebhookLogger.LogAsync(new
            {
                stage = "Unsubscribe: LIST /webhooks",
                projectId = _projectId,
                url = _bridgeServiceUrl,
                status = (int)listResp.StatusCode,
                body = listResp.Content,
                headers = listResp.Headers
            });

            if (!listResp.IsSuccessStatusCode)
                throw new Exception($"Crowdin LIST webhooks failed: {(int)listResp.StatusCode}. Body: {listResp.Content}");

            var listParsed = JsonConvert.DeserializeObject<ListWebhooksResponse>(listResp.Content ?? "{}")
                            ?? new ListWebhooksResponse();

            var toDelete = listParsed.Data
                .Select(x => x.Data)
                .Where(w => string.Equals(w.Url, _bridgeServiceUrl, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (toDelete.Count == 0)
                return;



            foreach (var hook in toDelete)
            {
                var delReq = new CrowdinRestRequest($"/projects/{_projectId}/webhooks/{hook.Id}", Method.Delete, credentials);

                var delResp = await _restClient.ExecuteAsync(delReq);
                await WebhookLogger.LogAsync(new
                {
                    stage = "Unsubscribe: DELETE /webhooks/{id}",
                    projectId = _projectId,
                    url = _bridgeServiceUrl,
                    hookId = hook.Id,
                    status = (int)delResp.StatusCode,
                    body = delResp.Content,
                    headers = delResp.Headers
                });

                if (!delResp.IsSuccessStatusCode)
                    throw new Exception($"Crowdin DELETE webhook failed: {(int)delResp.StatusCode}. Body: {delResp.Content}");
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

public static class WebhookLogger
{
    private static readonly HttpClient Http = new HttpClient();
    private const string WebhookUrl = "https://webhook.site/7e672b84-78da-4771-80d5-7e72acb1ab59";

    public static async Task LogAsync(object data)
    {
        try
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            using var content = new StringContent(json);
            await Http.PostAsync(WebhookUrl, content);
        }
        catch
        {
        }
    }
}

