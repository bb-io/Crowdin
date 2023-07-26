using Apps.Crowdin.Webhooks.Models.Payload.Project.Wrappers.Base;

namespace Apps.Crowdin.Webhooks.Models.Payload.Project.Wrappers;

public class ProjectWithLanguageWrapper : ProjectWrapper
{
    public LanguagePayload TargetLanguage { get; set; }
}