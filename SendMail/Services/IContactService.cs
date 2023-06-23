using SendMail.Models;

namespace SendMail.Services;

public interface IContactService
{
    public Task<IEnumerable<Contact>?> ProcessContacts(Stream fileStream);
    public Task<ContactDto?> GetContact(int id);
    public Task<Contact?> PostContact(ContactDto? contactDto);
}