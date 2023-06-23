using SendMail.Models;

namespace SendMail.Services;

public class ContactService : IContactService
{
    private readonly ICsvService _csvService;

    public ContactService(ICsvService csvService)
    {
        _csvService = csvService;
    }
    
    public IEnumerable<ContactDto>? ProcessContacts(Stream fileStream)
    {
        var records = _csvService.ProcessCsv<ContactDto>(fileStream);

        return records;
    }
}