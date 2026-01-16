using Apps.Crowdin.Webhooks.Bridge;
using Apps.Crowdin.Webhooks.Models.Inputs;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Crowdin.Api.Webhooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventType = Crowdin.Api.Webhooks.EventType;

namespace Apps.Crowdin.Webhooks.Handlers.Project.Task
{
    public class AllTasksReachedStatusHandler(InvocationContext invocationContext, [WebhookParameter(true)] ProjectWebhookInput input) : ProjectBridgeWebhookHandler(invocationContext, input)
    {
        protected override List<EventType> SubscriptionEvents => new() { EventType.TaskStatusChanged };
    }
}
