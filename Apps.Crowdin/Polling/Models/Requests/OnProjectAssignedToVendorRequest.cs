using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Polling.Models.Requests;

public class OnProjectAssignedToVendorRequest
{
    [Display("Group ID")] 
    public string? GroupId { get; set; }
}