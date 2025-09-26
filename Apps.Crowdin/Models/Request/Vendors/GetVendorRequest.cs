using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Apps.Crowdin.DataSourceHandlers.EnumHandlers;

namespace Apps.Crowdin.Models.Request.Vendors;

public class GetVendorRequest
{
    [Display("Name contains")]
    public string? NameContains { get; set; }

    [Display("Description contains")]
    public string? DescriptionContains { get; set; }

    [Display("Status")]
    [StaticDataSource(typeof(VendorStatusDataHandler))]
    public string? Status { get; set; }
}
