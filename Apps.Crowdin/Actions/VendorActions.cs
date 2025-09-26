using Apps.Crowdin.Api.RestSharp;
using Apps.Crowdin.Api.RestSharp.Enterprise;
using Apps.Crowdin.Invocables;
using Apps.Crowdin.Models.Request.Vendors;
using Apps.Crowdin.Models.Response;
using Apps.Crowdin.Models.Response.Vendors;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Crowdin.Api;
using RestSharp;

namespace Apps.Crowdin.Actions;

[ActionList]
public class VendorActions(InvocationContext invocationContext) : AppInvocable(invocationContext)
{
    [Action("[Enterprise] Find vendor", Description = "Gets the first vendor you already invited to your organization that matches the search criteria")]
    public async Task<GetVendorResponse?> GetVendor([ActionParameter] GetVendorRequest request)
    {
        var vendors = await ListVendors(request);
        return vendors.Data.FirstOrDefault();
    }

    [Action("[Enterprise] Search vendors", Description = "Get the list of the vendors you already invited to your organization")]
    public async Task<ListDataResponse<GetVendorResponse>> ListVendors([ActionParameter] GetVendorRequest request)
    {
        CheckAccessToEnterpriseAction();

        var client = new CrowdinEnterpriseRestClient(Creds);

        var items = await Paginator.Paginate(async (limit, offset) =>
        {
            var vendorsRequest = new CrowdinRestRequest(
                $"/vendors?limit={limit}&offset={offset}",
                Method.Get,
                InvocationContext.AuthenticationCredentialsProviders
            );

            var page = await ExceptionWrapper.ExecuteWithErrorHandling(
                () => client.ExecuteWithErrorHandling<ListDataResponse<GetVendorResponse>>(vendorsRequest)
            );

            return new ResponseList<GetVendorResponse> { Data = page.Data.ToList() };
        });

        var vendors = items.AsEnumerable();
        ApplyFilters(request, vendors);
        return new ListDataResponse<GetVendorResponse> { Data = vendors.ToList() };
    }

    private static void ApplyFilters(GetVendorRequest request, IEnumerable<GetVendorResponse> vendors)
    {
        if (!string.IsNullOrEmpty(request.Status))
            vendors = vendors.Where(v => v.Vendor.Status.ToString().Equals(request.Status, StringComparison.CurrentCultureIgnoreCase));

        if (!string.IsNullOrEmpty(request.NameContains))
            vendors = vendors.Where(v => v.Vendor.Name.Contains(request.NameContains, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrEmpty(request.DescriptionContains))
            vendors = vendors.Where(v => v.Vendor.Description?.Contains(request.DescriptionContains, StringComparison.OrdinalIgnoreCase) == true);
    }
}
