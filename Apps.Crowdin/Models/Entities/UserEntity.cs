using Apps.Crowdin.Webhooks.Models.Payload;
using Blackbird.Applications.Sdk.Common;
using Crowdin.Api.StringTranslations;

namespace Apps.Crowdin.Models.Entities;

public class UserEntity
{
    [Display("ID")] public string? Id { get; set; }
    public string? Username { get; set; }
    [Display("Full name")] public string? FullName { get; set; }
    [Display("Avatar url")] public string? AvatarUrl { get; set; }

    public UserEntity(User user)
    {
        Id = user.Id.ToString();
        Username = user.Username;
        FullName = user.FullName;
        AvatarUrl = user.AvatarUrl;
    }
    
    public UserEntity(UserPayload user)
    {
        Id = user.Id;
        Username = user.Username;
        FullName = user.FullName;
        AvatarUrl = user.AvatarUrl;
    }
    
}