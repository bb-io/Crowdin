using Apps.Crowdin.Webhooks.Handlers.Base;
using Apps.Crowdin.Webhooks.Models.Inputs;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Crowdin.Api.Webhooks;

namespace Apps.Crowdin.Webhooks.Handlers.Project.File;

public class FileAddedHandler : ProjectWebhookHandler
{
    protected override EventType SubscriptionEvent => EventType.FileAdded;

    public FileAddedHandler([WebhookParameter] ProjectWebhookInput input) : base(input)
    {
    }
}