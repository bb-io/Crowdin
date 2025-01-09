using Blackbird.Applications.Sdk.Common;
using Directory = Crowdin.Api.SourceFiles.Directory;

namespace Apps.Crowdin.Models.Entities;

public class DirectoryEntity
{
    [Display("Directory ID")] 
    public string? Id { get; set; }

    [Display("Branch ID")] 
    public string BranchId { get; set; }

    [Display("Parent directory ID")] 
    public string? DirectoryId { get; set; }

    [Display("Project ID")] 
    public string ProjectId { get; set; }

    public string Name { get; set; }

    public string? Title { get; set; }

    [Display("Created at")] 
    public DateTime CreatedAt { get; set; }

    public DirectoryEntity(Directory directory)
    {
        Id = directory.Id.ToString();
        BranchId = directory.BranchId.ToString();
        DirectoryId = directory.DirectoryId.ToString();
        ProjectId = directory.ProjectId.ToString();
        Name = directory.Name;
        Title = directory.Title;
        CreatedAt = directory.CreatedAt.DateTime;
    }

    public DirectoryEntity()
    { }
}