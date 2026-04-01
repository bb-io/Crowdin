using Apps.Crowdin.Models.Entities;

namespace Apps.Crowdin.Models.Response.Vendors;

public record ListVendorsResponse(VendorEntity[] Vendors);