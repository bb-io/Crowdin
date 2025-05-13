using Newtonsoft.Json;

namespace Apps.Crowdin.Webhooks.Bridge.Models
{
    public record CreateWebhookRequest(
        [property: JsonProperty("url")] string Url,
        [property: JsonProperty("events")] IEnumerable<string> SubscribedEvents);
}
