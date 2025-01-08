using Blackbird.Applications.Sdk.Common;
using Crowdin.Api.Users;

namespace Apps.Crowdin.Models.Entities;

public class UserEnterpriseEntity(UserEnterprise user)
{
    [Display("User ID")] 
    public string Id { get; set; } = user.Id.ToString();
    
    public string Username { get; set; } = user.Username;
    
    public string Email { get; set; } = user.Email;
    
    [Display("First name")] 
    public string FirstName { get; set; } = user.FirstName;
    
    [Display("Last name")] 
    public string LastName { get; set; } = user.LastName;

    public string Status { get; set; } = user.Status.ToString();
    
    [Display("Avatar url")] 
    public string AvatarUrl { get; set; } = user.AvatarUrl;
    
    [Display("Created at")] 
    public DateTime CreatedAt { get; set; } = user.CreatedAt.DateTime;
    
    [Display("Last seen")] 
    public DateTime? LastSeen { get; set; } = user.LastSeen?.DateTime;
    
    [Display("Is two factor enabled")] 
    public bool IsTwoFactorEnabled { get; set; } = user.TwoFactor is UserTwoFactorStatus.Enabled;
    
    public string Timezone { get; set; } = user.TimeZone;

    [Display("Is admin")]
    public bool IsAdmin { get; set; } = user.IsAdmin;
}