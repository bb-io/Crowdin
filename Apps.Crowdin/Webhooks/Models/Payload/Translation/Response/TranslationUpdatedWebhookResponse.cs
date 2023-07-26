using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Webhooks.Models.Payload.Translation.Wrappers;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Webhooks.Models.Payload.Translation.Response;

public class TranslationUpdatedWebhookResponse : CrowdinWebhookResponse<TranslationUpdatedWrapper>
{
    [Display("Old translation")]
    public TranslationEntity OldTranslation { get; set; }
    
    [Display("New translation")]
    public TranslationEntity NewTranslation { get; set; }
    
    public override void ConfigureResponse(TranslationUpdatedWrapper wrapper)
    {
        OldTranslation = new(wrapper.OldTranslation);
        NewTranslation = new(wrapper.NewTranslation);
    }
}