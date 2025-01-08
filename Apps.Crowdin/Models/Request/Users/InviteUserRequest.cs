using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.Users;

public class InviteUserRequest
{
    public string Email { get; set; }
    
    [Display("First name")]
    public string? FirstName { get; set; }
    
    [Display("Last name")]
    public string? LastName { get; set; }
    
    public string? Timezone { get; set; }
    
    [Display("Is admin")]
    public bool? IsAdmin { get; set; }
}