using SendMail.Models.ContactModels;

namespace SendMail.Models.UserModels;

public class UserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public IEnumerable<Contact>? Contacts { get; set; }
}
