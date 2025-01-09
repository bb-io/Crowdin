using Apps.Crowdin.Models.Entities;

namespace Apps.Crowdin.Models.Response.Directory;

public record ListDirectoriesResponse(DirectoryEntity[] Directories);