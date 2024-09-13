namespace Apps.Crowdin.Models.Entities;

public class AssigneeEntity
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public DateTime? GivenAccessAt { get; set; }
    public string FullName => string.IsNullOrWhiteSpace(FirstName) ? Username : $"{FirstName} {LastName}";
}