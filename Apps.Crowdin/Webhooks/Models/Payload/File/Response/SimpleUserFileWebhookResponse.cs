using Apps.Crowdin.Webhooks.Models.Payload.File.Wrappers;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Webhooks.Models.Payload.File.Response;

public class SimpleFileWebhookResponse : CrowdinWebhookResponse<SimpleUserFileWrapper>
{
    [Display("Project name")] public string Project { get; set; }

    [Display("Project ID")] public string ProjectId { get; set; }

    [Display("File ID")] public string FileId { get; set; }

    [Display("File path")] public string File { get; set; }

    [Display("User ID")] public string UserId { get; set; }

    [Display("User full name")] public string User { get; set; }

    public override void ConfigureResponse(SimpleUserFileWrapper wrapper)
    {
        Project = wrapper.Project;
        ProjectId = wrapper.ProjectId;
        FileId = wrapper.FileId;
        File = wrapper.File;
        UserId = wrapper.UserId;
        User = wrapper.User;
    }
}