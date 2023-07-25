using Apps.Crowdin.Models.Entities;
using Crowdin.Api.SourceFiles;

namespace Apps.Crowdin.Webhooks.Models.Payload.File.Response.Base;

public class FileWebhookResponse
{
    public FileEntity File { get; set; }

    public FileWebhookResponse(FileCollectionResource file)
    {
        File = new(file);
    }
}