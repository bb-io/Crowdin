using Apps.Crowdin.Webhooks.Models.Payload.File.Wrappers;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Webhooks.Models.Payload.File.Response;

public class SimpleLanguageFileWebhookResponse : CrowdinWebhookResponse<SimpleLanguageFileWrapper>
{
    [Display("Project name")] public string Project { get; set; }

    [Display("Project ID")] public string ProjectId { get; set; }

    [Display("File ID")] public string FileId { get; set; }

    [Display("File path")] public string File { get; set; }

    [Display("Target language ID")] public string Language { get; set; }

    public override void ConfigureResponse(SimpleLanguageFileWrapper wrapper)
    {
        Project = wrapper.Project;
        ProjectId = wrapper.ProjectId;
        FileId = wrapper.FileId;
        File = wrapper.File;
        Language = wrapper.Language;
    }
}