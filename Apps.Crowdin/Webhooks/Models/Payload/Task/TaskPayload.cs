using Crowdin.Api.ProjectsGroups;
using Crowdin.Api.StringTranslations;
using Crowdin.Api.Tasks;

namespace Apps.Crowdin.Webhooks.Models.Payload.Task;

public class TaskPayload
{
    public string Id { get; set; }
    public string Type { get; set; }
    public string Vendor { get; set; }
    public string Status { get; set; }
    public string Title { get; set; }
    public IEnumerable<User> Assignees { get; set; }
    public IEnumerable<int> FileIds { get; set; }
    public TaskProgress Progress { get; set; }
    public string Description { get; set; }
    public string TranslationUrl { get; set; }
    public DateTime? Deadline { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public LanguagePayload SourceLanguage { get; set; }
    public LanguagePayload TargetLanguage { get; set; }
    public EnterpriseProject Project { get; set; }
    public User TaskCreator { get; set; }
}