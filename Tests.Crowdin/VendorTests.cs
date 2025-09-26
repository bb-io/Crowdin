using Tests.Crowdin.Base;
using Apps.Crowdin.Actions;
using Apps.Crowdin.Models.Request.Vendors;

namespace Tests.Crowdin;

[TestClass]
public class VendorTests : TestBase
{
    [TestMethod]
    public async Task ListVendors_WithoutFilters_ReturnsVendors()
    {
		// Arrange
		var actions = new VendorActions(InvocationContext);
		var request = new GetVendorRequest { };

		// Act
		var result = await actions.ListVendors(request);

		// Assert
		foreach (var vendor in result.Data)
		{
            Console.WriteLine($"{vendor.Vendor.Id} - {vendor.Vendor.Name} - {vendor.Vendor.Status}");
		}
		Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task ListVendors_WithNameFilter_ReturnsVendors()
    {
        // Arrange
        var actions = new VendorActions(InvocationContext);
        var request = new GetVendorRequest { NameContains = "Localize" };

        // Act
        var result = await actions.ListVendors(request);

        // Assert
        foreach (var vendor in result.Data)
        {
            Console.WriteLine($"{vendor.Vendor.Id} - {vendor.Vendor.Name} - {vendor.Vendor.Status}");
        }
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task ListVendors_WithNameAndDescriptionFilter_ReturnsVendors()
    {
        // Arrange
        var actions = new VendorActions(InvocationContext);
        var request = new GetVendorRequest { NameContains = "l", DescriptionContains = "deliver" };

        // Act
        var result = await actions.ListVendors(request);

        // Assert
        foreach (var vendor in result.Data)
        {
            Console.WriteLine($"{vendor.Vendor.Id} - {vendor.Vendor.Name} - {vendor.Vendor.Status}");
        }
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task ListVendors_WithStatusFilter_ReturnsVendors()
    {
        // Arrange
        var actions = new VendorActions(InvocationContext);
        var request = new GetVendorRequest { Status = "confirmed" };

        // Act
        var result = await actions.ListVendors(request);

        // Assert
        foreach (var vendor in result.Data)
        {
            Console.WriteLine($"{vendor.Vendor.Id} - {vendor.Vendor.Name} - {vendor.Vendor.Status}");
        }
        Assert.IsNotNull(result);
    }
}
