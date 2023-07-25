using Apps.Crowdin.Webhooks.Models.Payload.Project.Wrappers.Base;
using Crowdin.Api.StringTranslations;

namespace Apps.Crowdin.Webhooks.Models.Payload.Project.Wrappers;

public class ProjectWithUserWrapper : ProjectWrapper
{
    public User User { get; set; }
}