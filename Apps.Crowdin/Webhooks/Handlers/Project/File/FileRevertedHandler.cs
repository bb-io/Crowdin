using Apps.Crowdin.Webhooks.Handlers.Base;
using Apps.Crowdin.Webhooks.Models.Inputs;
using Blackbird.Applications.Sdk.Common.Webhooks;
using EventType = Crowdin.Api.Webhooks.EventType;

namespace Apps.Crowdin.Webhooks.Handlers.Project.File;

public class FileRevertedHandler : ProjectWebhookHandler
{
    protected override EventType SubscriptionEvent => EventType.FileReverted;

    public FileRevertedHandler([WebhookParameter(true)] ProjectWebhookInput input) : base(input)
    {
    }
}