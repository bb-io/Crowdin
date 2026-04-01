using Apps.Crowdin.Models.Entities;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Response.Vendors;

public class GetVendorResponse
{
    [Display("Vendor")]
    public VendorEntity? Vendor { get; set; }
}
