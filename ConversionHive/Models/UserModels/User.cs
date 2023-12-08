using ConversionHive.Models.ContactModels;

namespace ConversionHive.Models.UserModels;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public IEnumerable<Contact>? Contacts { get; set; }
}
