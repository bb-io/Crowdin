using Newtonsoft.Json;
using Crowdin.Api.Vendors;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Response.Vendors;

public class GetVendorResponse
{
    [JsonProperty("data")]
    [Display("Vendors")]
    public Vendor Vendor { get; set; }
}
