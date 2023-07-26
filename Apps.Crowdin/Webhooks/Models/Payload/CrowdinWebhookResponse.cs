namespace Apps.Crowdin.Webhooks.Models.Payload;

public abstract class CrowdinWebhookResponse<T>
{
    public abstract void ConfigureResponse(T wrapper);
}