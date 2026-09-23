using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.TranslationMemory;

public class ListTranslationMemoryRequest
{
    [Display("User ID", Description = "Supported for Crowdin Basic plan only")] 
    public string? UserId { get; set; }
    
    [Display("Group ID", Description = "Supported for Crowdin Enterprise plan only")] 
    public string? GroupId { get; set; }
}