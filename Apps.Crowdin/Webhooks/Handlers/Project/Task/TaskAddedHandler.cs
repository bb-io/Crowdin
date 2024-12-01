using Apps.Crowdin.Webhooks.Handlers.Base;
using Apps.Crowdin.Webhooks.Models.Inputs;
using Blackbird.Applications.Sdk.Common.Webhooks;
using EventType = Crowdin.Api.Webhooks.EventType;

namespace Apps.Crowdin.Webhooks.Handlers.Project.Task;

public class TaskAddedHandler : ProjectWebhookHandler
{
    protected override EventType SubscriptionEvent => EventType.TaskAdded;

    public TaskAddedHandler([WebhookParameter(true)] ProjectWebhookInput input) : base(input)
    {
    }
}