using Crowdin.Api.SourceFiles;

namespace Apps.Crowdin.Webhooks.Models.Payload.File.Wrappers.Base;

public class FileWrapper
{
    public FileCollectionResource File { get; set; }
}