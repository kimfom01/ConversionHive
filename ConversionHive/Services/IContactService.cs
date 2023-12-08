using ConversionHive.Dtos.ContactDto;

namespace ConversionHive.Services;

public interface IContactService
{
    public Task<IEnumerable<CreateContactResponseDto>?> ProcessContacts(string authorization, Stream fileStream);
    public Task<CreateContactDto?> GetContact(int id);
    public Task<CreateContactResponseDto?> PostContact(CreateContactDto? contactDto);
}