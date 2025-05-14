using Apps.Crowdin.Webhooks.Bridge;
using Apps.Crowdin.Webhooks.Handlers.Base;
using Apps.Crowdin.Webhooks.Models.Inputs;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using EventType = Crowdin.Api.Webhooks.EventType;

namespace Apps.Crowdin.Webhooks.Handlers.Project.File;

public class FileAddedHandler(InvocationContext invocationContext, [WebhookParameter(true)] ProjectWebhookInput input) : ProjectBridgeWebhookHandler(invocationContext, input)
{
    protected override List<EventType> SubscriptionEvents => new() { EventType.FileAdded };

}