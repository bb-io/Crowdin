using Apps.Crowdin.Models.Dtos;
using Blackbird.Applications.Sdk.Common;
using Crowdin.Api.Tasks;
using Newtonsoft.Json;

namespace Apps.Crowdin.Models.Entities;

public class TaskEntity
{
    [Display("Task ID")]
    public string Id { get; set; }

    [Display("Project ID")]
    public string ProjectId { get; set; }

    [Display("Creator ID")]
    public string CreatorId { get; set; }

    public string Status { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string Vendor { get; set; }

    public IEnumerable<string> Assignees { get; set; }

    [Display("File IDs")]
    public IEnumerable<string> FileIds { get; set; }

    [Display("Source language ID")]
    public string SourceLanguageId { get; set; }

    [Display("Target language ID")]
    public string TargetLanguageId { get; set; }

    public DateTime? Deadline { get; set; }

    [Display("Created at")]
    public DateTime CreatedAt { get; set; }

    [Display("Fields")]
    public IEnumerable<FieldEntity> Fields => FieldsDict.Select(x => new FieldEntity(x.Key, x.Value));

    [DefinitionIgnore, JsonProperty("fields")]
    public Dictionary<string, object> FieldsDict { get; set; } = [];

    public TaskEntity(TaskResource taskResource)
    {
        Id = taskResource.Id.ToString();
        ProjectId = taskResource.ProjectId.ToString();
        CreatorId = taskResource.CreatorId.ToString();
        Status = taskResource.Status.ToString();
        Title = taskResource.Title;
        Description = taskResource.Description;
        Assignees = taskResource.Assignees.Select(x => x.FullName);
        Vendor = taskResource.Vendor;
        FileIds = taskResource.FileIds.Select(x => x.ToString());
        SourceLanguageId = taskResource.SourceLanguageId;
        TargetLanguageId = taskResource.TargetLanguageId;
        Deadline = taskResource.DeadLine.DateTime;
        CreatedAt = taskResource.CreatedAt.DateTime;
    }

    public TaskEntity(TaskResourceDto taskResource)
    {
        Id = taskResource.Id.ToString();
        ProjectId = taskResource.ProjectId.ToString();
        CreatorId = taskResource.CreatorId.ToString();
        Status = taskResource.Status.ToString();
        Title = taskResource.Title;
        Description = taskResource.Description;
        Assignees = taskResource.Assignees.Select(x => x.FullName);
        Vendor = taskResource.Vendor;
        FileIds = taskResource.FileIds.Select(x => x.ToString());
        SourceLanguageId = taskResource.SourceLanguageId;
        TargetLanguageId = taskResource.TargetLanguageId;
        Deadline = taskResource.DeadLine;
        CreatedAt = taskResource.CreatedAt;
    }
}