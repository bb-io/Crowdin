using Apps.Crowdin.Webhooks.Models.Payload.Translation.Wrappers;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Webhooks.Models.Payload.Translation.Response;

public class SimpleTranslationWebhookResponse : CrowdinWebhookResponse<SimpleTranslationWrapper>
{
    [Display("Project name")]
    public string Project { get; set; }
    
    [Display("Project ID")]
    public string ProjectId { get; set; }
    
    [Display("Target language ID")]
    public string Language { get; set; }
    
    [Display("Source string ID")] 
    public string SourceStringId { get; set; }
    
    [Display("Old translation ID")]
    public string OldTranslationId { get; set; }
    
    [Display("New translation ID")]
    public string NewTranslationId { get; set; }
    
    public string User { get; set; }
    
    [Display("User ID")]
    public string UserId { get; set; }
    
    [Display("Translation provider")]
    public string? Provider { get; set; }
    
    [Display("Is pre-translated")]
    public bool IsPreTranslated { get; set; }
    
    [Display("File ID")]
    public bool FileId { get; set; }
    
    public bool File { get; set; }
    
    public override void ConfigureResponse(SimpleTranslationWrapper wrapper)
    {
        Project = wrapper.Project;
        ProjectId = wrapper.ProjectId;
        OldTranslationId = wrapper.OldTranslationId;
        NewTranslationId = wrapper.NewTranslationId;
        Language = wrapper.Language;
        SourceStringId = wrapper.SourceStringId;
        User = wrapper.User;
        UserId = wrapper.UserId;
        Provider = wrapper.Provider;
        IsPreTranslated = wrapper.IsPreTranslated;
        FileId = wrapper.FileId;
        File = wrapper.File;
    }
}