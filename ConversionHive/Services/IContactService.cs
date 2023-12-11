using ConversionHive.Dtos.ContactDto;

namespace ConversionHive.Services;

public interface IContactService
{
    public Task<List<ReadContactDto>> PostContactsCsv(string authorization, Stream fileStream);
    public Task<ReadContactDto?> GetContact(string authorization, int contactId);
    public Task<ReadContactDto> PostContact(string authorization, CreateContactDto? contactDto);
}