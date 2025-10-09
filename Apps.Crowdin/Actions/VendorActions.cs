using Apps.Crowdin.Invocables;
using Apps.Crowdin.Models.Request.Vendors;
using Apps.Crowdin.Models.Response.Vendors;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Crowdin.Api;
using Crowdin.Api.Vendors;

namespace Apps.Crowdin.Actions;

[ActionList("Vendors")]
public class VendorActions(InvocationContext invocationContext) : AppInvocable(invocationContext)
{
    [Action("[Enterprise] Find vendor", Description = "Gets the first vendor you already invited to your organization that matches the search criteria")]
    public async Task<GetVendorResponse?> GetVendor([ActionParameter] GetVendorRequest request)
    {
        var vendors = await ListVendors(request);
        return new GetVendorResponse { Vendor = vendors.Vendors.FirstOrDefault() };
    }

    [Action("[Enterprise] Search vendors", Description = "Get the list of the vendors you already invited to your organization")]
    public async Task<ListVendorsResponse> ListVendors([ActionParameter] GetVendorRequest request)
    {
        CheckAccessToEnterpriseAction();

        var response = await Paginator.Paginate(ListVendorsAsync);
        var vendors = response.Select(x => new VendorEntity(x)).AsEnumerable();

        var filtered = ApplyFilters(request, vendors);
        return new(filtered);
    }

    private static VendorEntity[] ApplyFilters(GetVendorRequest request, IEnumerable<VendorEntity> vendors)
    {
        if (!string.IsNullOrEmpty(request.Status))
            vendors = vendors.Where(v => v.Status.ToString().Equals(request.Status, StringComparison.CurrentCultureIgnoreCase));

        if (!string.IsNullOrEmpty(request.NameContains))
            vendors = vendors.Where(v => v.Name.Contains(request.NameContains, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrEmpty(request.DescriptionContains))
            vendors = vendors.Where(v => v.Description?.Contains(request.DescriptionContains, StringComparison.OrdinalIgnoreCase) == true);

        return vendors.ToArray();
    }

    private Task<ResponseList<Vendor>> ListVendorsAsync(int limit = 25, int offset = 0)
    {
        return ExceptionWrapper.ExecuteWithErrorHandling(() => SdkClient.Vendors.ListVendors(limit, offset));
    }
}
