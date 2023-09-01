using Crowdin.Api.StringTranslations;

namespace Apps.Crowdin.Webhooks.Models.Payload.String.Wrappers;

public class SourceStringWrapper
{
    public StringPayload String { get; set; }
    public User User { get; set; }
}