using Apps.Crowdin.Webhooks.Models.Payload.File.Wrappers.Base;
using Crowdin.Api.Languages;

namespace Apps.Crowdin.Webhooks.Models.Payload.File.Wrappers;

public class FileWithLanguageWrapper : FileWrapper
{
    public Language TargetLanguage { get; set; }
}