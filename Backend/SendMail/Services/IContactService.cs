using SendMail.Models.ContactModels;

namespace SendMail.Services;

public interface IContactService
{
    public Task<IEnumerable<CreateContactResponseDto>?> ProcessContacts(Stream fileStream);
    public Task<CreateContactDto?> GetContact(int id);
    public Task<CreateContactResponseDto?> PostContact(CreateContactDto? contactDto);
}