using Crowdin.Api.Vendors;

public class VendorEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }

    public VendorEntity(Vendor vendor)
    {
        Id = vendor.Id;
        Name = vendor.Name;
        Description = vendor.Description;
        Status = vendor.Status.ToString().ToLower();
    }
}