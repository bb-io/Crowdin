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
		foreach (var vendor in result)
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
        var request = new GetVendorRequest { NameContains = "l" };

        // Act
        var result = await actions.ListVendors(request);

        // Assert
        foreach (var vendor in result)
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
        foreach (var vendor in result)
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
        var request = new GetVendorRequest { Status = "pending" };

        // Act
        var result = await actions.ListVendors(request);

        // Assert
        foreach (var vendor in result)
        {
            Console.WriteLine($"{vendor.Vendor.Id} - {vendor.Vendor.Name} - {vendor.Vendor.Status}");
        }
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetVendor_WithoutFilters_ReturnsVendor()
    {
        // Arrange
        var actions = new VendorActions(InvocationContext);
        var request = new GetVendorRequest { };

        // Act
        var result = await actions.GetVendor(request);

        // Assert
        Assert.IsNotNull(result);
        Console.WriteLine($"{result.Vendor.Id} - {result.Vendor.Name} - {result.Vendor.Status}");
    }

    [TestMethod]
    public async Task GetVendor_WithNameFilter_ReturnsVendor()
    {
        // Arrange
        var actions = new VendorActions(InvocationContext);
        var request = new GetVendorRequest { NameContains = "l" };

        // Act
        var result = await actions.GetVendor(request);

        // Assert
        Assert.IsNotNull(result);
        Console.WriteLine($"{result.Vendor.Id} - {result.Vendor.Name} - {result.Vendor.Status}");
    }

    [TestMethod]
    public async Task GetVendor_WithNameAndDescriptionFilter_ReturnsVendor()
    {
        // Arrange
        var actions = new VendorActions(InvocationContext);
        var request = new GetVendorRequest { NameContains = "l", DescriptionContains = "company" };

        // Act
        var result = await actions.GetVendor(request);

        // Assert
        Assert.IsNotNull(result);
        Console.WriteLine($"{result.Vendor.Id} - {result.Vendor.Name} - {result.Vendor.Status}");
    }
    
    [TestMethod]
    public async Task GetVendor_WithStatusFilter_ReturnsVendor()
    {
        // Arrange
        var actions = new VendorActions(InvocationContext);
        var request = new GetVendorRequest { Status = "confirmed" };

        // Act
        var result = await actions.GetVendor(request);

        // Assert
        Assert.IsNotNull(result);
        Console.WriteLine($"{result.Vendor.Id} - {result.Vendor.Name} - {result.Vendor.Status}");
    }
}
