using Crowdin.Api.Vendors;
using Blackbird.Applications.Sdk.Common;

public class VendorEntity
{
    [Display("Vendor ID")]
    public int Id { get; set; }

    [Display("Name")]
    public string Name { get; set; }

    [Display("Description")]
    public string? Description { get; set; }

    [Display("Status")]
    public string Status { get; set; }

    public VendorEntity(Vendor vendor)
    {
        Id = vendor.Id;
        Name = vendor.Name;
        Description = string.IsNullOrWhiteSpace(vendor.Description) ? null : vendor.Description;
        Status = vendor.Status.ToString();
    }
}