using ConversionHive.Dtos.ContactDto;

namespace ConversionHive.Services;

public interface IContactService
{
    public Task<IEnumerable<CreateContactDto>?> PostContactsCsv(string authorization, Stream fileStream);
    public Task<ReadContactDto?> GetContact(int id);
    public Task<ReadContactDto?> PostContact(CreateContactDto? contactDto);
}