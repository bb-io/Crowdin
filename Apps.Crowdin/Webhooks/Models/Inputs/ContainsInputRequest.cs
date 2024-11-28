
using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Webhooks.Models.Inputs
{
    public class ContainsInputRequest
    {
        [Display("Filter text", Description = "If specified, the webhook will only trigger for comments containing this text.")]
        public string? Text { get; set; }
    }
}
