using Blackbird.Applications.Sdk.Common;
using Crowdin.Api.ProjectsGroups;

namespace Apps.Crowdin.Models.Entities;

public class ProjectEntity
{
    [Display("ID")]
    public string Id { get; set; }
    
    [Display("User ID")]
    public string UserId { get; set; }
    
    [Display("Source language ID")]
    public string SourceLanguageId { get; set; }

    [Display("Target language IDs")]
    public string[] TargetLanguageIds { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Logo { get; set; }

    [Display("Created at")]
    public DateTime CreatedAt { get; set; }

    [Display("Last activity")]
    public DateTime? LastActivity { get; set; }
    
    public ProjectEntity(ProjectBase project)
    {
        Id = project.Id.ToString();
        UserId = project.UserId.ToString();
        SourceLanguageId = project.SourceLanguageId;
        TargetLanguageIds = project.TargetLanguageIds;
        Name = project.Name;
        Description = project.Description;
        Logo = project.Logo;
        CreatedAt = project.CreatedAt.DateTime;
        LastActivity = project.LastActivity?.DateTime;
    }
}