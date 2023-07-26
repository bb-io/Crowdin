using Apps.Crowdin.Webhooks.Models.Payload.File.Wrappers.Base;

namespace Apps.Crowdin.Webhooks.Models.Payload.File.Wrappers;

public class FileWithLanguageWrapper : FileWrapper
{
    public LanguagePayload TargetLanguage { get; set; }
}