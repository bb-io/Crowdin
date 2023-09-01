namespace Apps.Crowdin.Webhooks.Models.Payload;

public class UserPayload
{
    public string? Id { get; set; }

    public string? Username { get; set; }

    public string? FullName { get; set; }

    public string? AvatarUrl { get; set; }
}