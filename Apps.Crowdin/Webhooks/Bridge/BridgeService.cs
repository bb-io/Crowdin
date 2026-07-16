using Apps.Crowdin.Webhooks.Bridge.Models;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using RestSharp;

namespace Apps.Crowdin.Webhooks.Bridge
{
    public class BridgeService
    {
        private string BridgeServiceUrl { get; set; }

        public BridgeService(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders, string bridgeServiceUrl)
        {
            BridgeServiceUrl = bridgeServiceUrl;
        }

        public async Task Subscribe(string @event, string projectId, string url)
        {
            var client = new RestClient(BridgeServiceUrl);
            var request = new RestRequest($"/{projectId}/{@event}", Method.Post);
            request.AddHeader("Blackbird-Token", ApplicationConstants.BlackbirdToken);
            request.AddBody(url);

            var response = await client.ExecuteAsync(request);
            if (!response.IsSuccessful)
                throw new PluginApplicationException($"Failed to subscribe to event {@event} for project {projectId}");
        }

        public async Task Unsubscribe(string @event, string projectId, string url)
        {
            var client = new RestClient(BridgeServiceUrl);
            var requestGet = new RestRequest($"/{projectId}/{@event}")
                .AddHeader("Blackbird-Token", ApplicationConstants.BlackbirdToken);
            var webhooks = await client.GetAsync<List<BridgeGetResponse>>(requestGet);
            
            var webhook = webhooks?.FirstOrDefault(w => w.Value == url);
            if (webhook == null) 
                return;
            
            var requestDelete = new RestRequest($"/{projectId}/{@event}/{webhook.Id}", Method.Delete)
                .AddHeader("Blackbird-Token", ApplicationConstants.BlackbirdToken);
            var responseDelete = await client.DeleteAsync(requestDelete);
            if (!responseDelete.IsSuccessful)
                throw new PluginApplicationException($"Failed to subscribe to event {@event} for project {projectId}");
        }

        public async Task<bool> IsAnySubscriberExist(string @event, string projectId)
        {
            var client = new RestClient(BridgeServiceUrl);
            var request = new RestRequest($"/{projectId}/{@event}")
                .AddHeader("Blackbird-Token", ApplicationConstants.BlackbirdToken);
            
            var response = await client.GetAsync<List<BridgeGetResponse>>(request);
            return response?.Count > 0;
        }
    }
}
