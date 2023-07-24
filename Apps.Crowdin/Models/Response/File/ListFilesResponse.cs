using Apps.Crowdin.Models.Entities;

namespace Apps.Crowdin.Models.Response.File;

public record ListFilesResponse(FileEntity[] Files);