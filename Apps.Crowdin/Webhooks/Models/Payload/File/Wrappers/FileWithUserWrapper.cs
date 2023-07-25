using Apps.Crowdin.Webhooks.Models.Payload.File.Wrappers.Base;
using Crowdin.Api.StringTranslations;

namespace Apps.Crowdin.Webhooks.Models.Payload.File.Wrappers;

public class FileWithUserWrapper : FileWrapper
{
    public User User { get; set; }
}