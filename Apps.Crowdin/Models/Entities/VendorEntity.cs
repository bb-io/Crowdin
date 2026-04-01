using Crowdin.Api.Vendors;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Entities;

public class VendorEntity(Vendor vendor)
{
    [Display("Vendor ID")]
    public long Id { get; set; } = vendor.Id;

    [Display("Name")]
    public string Name { get; set; } = vendor.Name;

    [Display("Description")]
    public string? Description { get; set; } = string.IsNullOrWhiteSpace(vendor.Description) ? null : vendor.Description;

    [Display("Status")]
    public string Status { get; set; } = vendor.Status.ToString();
}