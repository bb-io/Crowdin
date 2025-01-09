using Apps.Crowdin.Models.Entities;

namespace Apps.Crowdin.Models.Response.ProjectGroups;

public record ListGroupsResponse(GroupEntity[] Groups);