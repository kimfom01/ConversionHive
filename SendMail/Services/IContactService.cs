using SendMail.Models;

namespace SendMail.Services;

public interface IContactService
{
    public IEnumerable<ContactDto>? ProcessContacts(Stream fileStream);
}