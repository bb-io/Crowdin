using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Webhooks.Models.Payload.File.Wrappers;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Webhooks.Models.Payload.File.Response;

public class FileWithLanguageWebhookResponse : CrowdinWebhookResponse<FileWithLanguageWrapper>
{
    public FileEntity File { get; set; }
    [Display("Target language ID")] public string LanguageId { get; set; }


    public override void ConfigureResponse(FileWithLanguageWrapper wrapper)
    {
        File = new(wrapper.File);
        LanguageId = wrapper.TargetLanguage.Id;
    }
}