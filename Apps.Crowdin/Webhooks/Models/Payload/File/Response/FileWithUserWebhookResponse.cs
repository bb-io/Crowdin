using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Webhooks.Models.Payload.File.Response.Base;
using Apps.Crowdin.Webhooks.Models.Payload.File.Wrappers;

namespace Apps.Crowdin.Webhooks.Models.Payload.File.Response;

public class FileWithUserWebhookResponse : FileWebhookResponse
{
    public UserEntity User { get; set; }

    public FileWithUserWebhookResponse(FileWithUserWrapper wrapper) : base(wrapper.File)
    {
        User = new(wrapper.User);
    }
}