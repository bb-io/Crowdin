using Apps.Crowdin.Webhooks.Models.Payload.Project.Wrappers.Base;
using Crowdin.Api.Languages;

namespace Apps.Crowdin.Webhooks.Models.Payload.Project.Wrappers;

public class ProjectWithLanguageWrapper : ProjectWrapper
{
    public Language TargetLanguage { get; set; }
}