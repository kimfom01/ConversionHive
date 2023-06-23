using SendMail.Models;

namespace SendMail.Services;

public interface IContactService
{
    public IEnumerable<ContactDto>? ProcessContacts(Stream fileStream);
    public Task<ContactDto?> GetContact(int id);
}