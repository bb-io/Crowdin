using Apps.Crowdin.Converters;
using Blackbird.Applications.Sdk.Common;
using Crowdin.Api.ProjectsGroups;
using Newtonsoft.Json;

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
    public IEnumerable<string> TargetLanguageIds { get; set; }
    
    [Display("Target language ID")]
    public string? TargetLanguageId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Logo { get; set; }

    [Display("Created at")]
    public DateTime CreatedAt { get; set; }

    [Display("Last activity")]
    public DateTime? LastActivity { get; set; }

    [Display("Fields"), JsonProperty("fields"), JsonConverter(typeof(FieldsConverter))]
    public IEnumerable<FieldEntity> Fields { get; set; } = [];

    public ProjectEntity() { }

    public ProjectEntity(ProjectBase project)
    {
        Id = project.Id.ToString();
        UserId = project.UserId.ToString();
        SourceLanguageId = project.SourceLanguageId;
        TargetLanguageIds = project.TargetLanguageIds;
        TargetLanguageId = project.TargetLanguageIds.FirstOrDefault();
        Name = project.Name;
        Description = project.Description;
        Logo = project.Logo;
        CreatedAt = project.CreatedAt.DateTime;
        LastActivity = project.LastActivity?.DateTime;
    }
}