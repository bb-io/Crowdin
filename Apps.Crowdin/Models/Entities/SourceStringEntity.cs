using Blackbird.Applications.Sdk.Common;
using Crowdin.Api.SourceStrings;

namespace Apps.Crowdin.Models.Entities;

public class SourceStringEntity
{
    [Display("ID")] public string Id { get; set; }
    [Display("Project ID")] public string ProjectId { get; set; }
    [Display("File ID")] public string FileId { get; set; }
    [Display("Branch ID")] public string? BranchId { get; set; }
    [Display("Directory ID")] public string? DirectoryId { get; set; }
    [Display("Web URL")] public string? WebUrl { get; set; }
    public string Identifier { get; set; }
    public string Text { get; set; }
    public string Type { get; set; }
    public string Context { get; set; }
    [Display("Created at")] public DateTime CreatedAt { get; set; }

    public SourceStringEntity(SourceString sourceString)
    {
        Id = sourceString.Id.ToString();
        ProjectId = sourceString.ProjectId.ToString();
        FileId = sourceString.FileId.ToString();
        BranchId = sourceString.BranchId?.ToString();
        DirectoryId = sourceString.DirectoryId?.ToString();
        Identifier = sourceString.Identifier;
        Text = sourceString.Text.ToString();
        Type = sourceString.Type;
        Context = sourceString.Context;
        CreatedAt = sourceString.CreatedAt.DateTime;
        WebUrl = sourceString.WebUrl;
    }
}