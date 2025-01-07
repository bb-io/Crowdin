using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Crowdin.Models.Response.Project;

public record DownloadFilesResponse(List<FileReference> Files);