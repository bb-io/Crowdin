using Apps.Crowdin.Webhooks.Handlers.Base;
using Apps.Crowdin.Webhooks.Models.Inputs;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Crowdin.Api.Webhooks;

namespace Apps.Crowdin.Webhooks.Handlers.Project.StringComment;

public class StringCommentRestoredHandler : ProjectWebhookHandler
{
    protected override EventType SubscriptionEvent => EventType.StringCommentRestored;

    public StringCommentRestoredHandler([WebhookParameter] ProjectWebhookInput input) : base(input)
    {
    }
}