using Blackbird.Applications.Sdk.Common;
using Crowdin.Api.Tasks;

namespace Apps.Crowdin.Models.Entities;

public class TaskEntity
{
    [Display("ID")]
    public string Id { get; set; }

    [Display("Project ID")]
    public string ProjectId { get; set; }

    [Display("Creator ID")]
    public string CreatorId { get; set; }

    public string Status { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string Vendor { get; set; }

    [Display("File IDs")]
    public IEnumerable<string> FileIds { get; set; }

    [Display("Source language ID")]
    public string SourceLanguageId { get; set; }

    [Display("Target language ID")]
    public string TargetLanguageId { get; set; }

    public DateTime Deadline { get; set; }

    [Display("Created at")]
    public DateTime CreatedAt { get; set; }
    
    public TaskEntity(TaskResource taskResource)
    {
        Id = taskResource.Id.ToString();
        ProjectId = taskResource.ProjectId.ToString();
        CreatorId = taskResource.CreatorId.ToString();
        Status = taskResource.Status.ToString();
        Title = taskResource.Title;
        Description = taskResource.Description;
        Vendor = taskResource.Vendor;
        FileIds = taskResource.FileIds.Select(x => x.ToString());
        SourceLanguageId = taskResource.SourceLanguageId;
        TargetLanguageId = taskResource.TargetLanguageId;
        Deadline = taskResource.DeadLine.DateTime;
        CreatedAt = taskResource.CreatedAt.DateTime;
    }
}