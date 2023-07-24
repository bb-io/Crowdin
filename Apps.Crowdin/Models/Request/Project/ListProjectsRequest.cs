using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.Project;

public class ListProjectsRequest
{
    [Display("User ID")] public string? UserId { get; set; }
    [Display("Group ID")] public string? GroupID { get; set; }
    [Display("Has manager access")] public bool? HasManagerAccess { get; set; }
}