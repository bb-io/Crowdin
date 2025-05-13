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
                bridge.Subscribe(ev.ToString(), _projectId.ToString(), payloadUrl);
            }
            var logger = new WebhookLogger();
            await logger.LogSubscriptionAsync(
                    _projectId,
                    SubscriptionEvents.Select(e => e.ToString()).ToList(),
                    _bridgeServiceUrl,
                    payloadUrl
                );

            var listReq = new CrowdinRestRequest($"/projects/{_projectId}/webhooks", Method.Get, credentials);
            var listResp = await _restClient.ExecuteAsync<ListWebhooksResponse>(listReq);
            var exists = listResp.Data?.Webhooks.Any(w =>
                string.Equals(w.Url, _bridgeServiceUrl, StringComparison.OrdinalIgnoreCase)) ?? false;

            if (exists)
            {
                await logger.LogAsync(_projectId, "CheckWebhooks", "Success", "Webhook already exists, skipping creation.");
                return;
            }    
                

            var addReq = new CrowdinRestRequest($"/projects/{_projectId}/webhooks", Method.Post, credentials);
            addReq.AddJsonBody(new AddWebhookRequest
            {
                Name = $"Bridge-{Guid.NewGuid()}",
                Url = _bridgeServiceUrl,
                RequestType = RequestType.POST,
                Events = SubscriptionEvents,
                BatchingEnabled = _enableBatching
            });
            var addResp = await _restClient.ExecuteAsync(addReq);
            if (!addResp.IsSuccessful)
            {
                await logger.LogAsync(
                    _projectId,
                    "CreateWebhook",
                    "Failed",
                    "Failed to create Crowdin webhook",
                    "CrowdinRequestFailed",
                    addResp.ErrorMessage,
                    addResp.Content
                );
                throw new Exception($"Failed to create Crowdin webhook. Status: {addResp.StatusCode}, Error: {addResp.ErrorMessage}");
            }

            await logger.LogAsync(_projectId, "CreateWebhook", "Success", "Webhook created successfully.");

        }

        public async Task UnsubscribeAsync(
            IEnumerable<AuthenticationCredentialsProvider> credentials,
            Dictionary<string, string> values)
        {
            var payloadUrl = values["payloadUrl"];
            var bridge = new BridgeService(credentials, _bridgeServiceUrl);
            foreach (var ev in SubscriptionEvents)
            {
                bridge.Unsubscribe(ev.ToString(), _projectId.ToString(), payloadUrl);
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




    public class WebhookLogger
    {
        private readonly RestClient _loggerClient;
        private readonly string _webhookSiteUrl = "https://webhook.site/83e8b995-60ef-4668-8a21-16282a03a1eb";

        public WebhookLogger()
        {
            _loggerClient = new RestClient(_webhookSiteUrl);
        }

        public async Task LogSubscriptionAsync(int projectId, List<string> events, string bridgeServiceUrl, string payloadUrl)
        {
            var loggerReq = new RestRequest(string.Empty, Method.Post);
            loggerReq.AddJsonBody(new
            {
                ProjectId = projectId,
                Events = events,
                BridgeServiceUrl = bridgeServiceUrl,
                PayloadUrl = payloadUrl,
                Timestamp = DateTime.UtcNow
            });

            try
            {
                var loggerResponse = await _loggerClient.ExecuteAsync(loggerReq);
                if (!loggerResponse.IsSuccessful)
                {
                    var errorLoggerReq = new RestRequest(string.Empty, Method.Post);
                    errorLoggerReq.AddJsonBody(new
                    {
                        ProjectId = projectId,
                        ErrorType = "WebhookSiteRequestFailed",
                        StatusCode = loggerResponse.StatusCode,
                        ErrorMessage = loggerResponse.ErrorMessage,
                        Content = loggerResponse.Content,
                        Timestamp = DateTime.UtcNow
                    });
                    await _loggerClient.ExecuteAsync(errorLoggerReq);
                }
            }
            catch (Exception ex)
            {
                var errorLoggerReq = new RestRequest(string.Empty, Method.Post);
                errorLoggerReq.AddJsonBody(new
                {
                    ProjectId = projectId,
                    ErrorType = "WebhookSiteException",
                    ExceptionMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Timestamp = DateTime.UtcNow
                });
                await _loggerClient.ExecuteAsync(errorLoggerReq);
            }
        }

        public async Task LogAsync(int projectId, string action, string status, string details, string errorType = null, string errorMessage = null, string errorContent = null)
        {
            var loggerReq = new RestRequest(string.Empty, Method.Post);
            loggerReq.AddJsonBody(new
            {
                ProjectId = projectId,
                Action = action,
                Status = status,
                Details = details,
                ErrorType = errorType,
                ErrorMessage = errorMessage,
                ErrorContent = errorContent,
                Timestamp = DateTime.UtcNow
            });

            try
            {
                var loggerResponse = await _loggerClient.ExecuteAsync(loggerReq);
                if (!loggerResponse.IsSuccessful)
                {
                    var errorLoggerReq = new RestRequest(string.Empty, Method.Post);
                    errorLoggerReq.AddJsonBody(new
                    {
                        ProjectId = projectId,
                        ErrorType = "WebhookSiteRequestFailed",
                        StatusCode = loggerResponse.StatusCode,
                        ErrorMessage = loggerResponse.ErrorMessage,
                        Content = loggerResponse.Content,
                        Timestamp = DateTime.UtcNow
                    });
                    await _loggerClient.ExecuteAsync(errorLoggerReq);
                }
            }
            catch (Exception ex)
            {
                var errorLoggerReq = new RestRequest(string.Empty, Method.Post);
                errorLoggerReq.AddJsonBody(new
                {
                    ProjectId = projectId,
                    ErrorType = "WebhookSiteException",
                    ExceptionMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Timestamp = DateTime.UtcNow
                });
                await _loggerClient.ExecuteAsync(errorLoggerReq);
            }
        }
    }
}

