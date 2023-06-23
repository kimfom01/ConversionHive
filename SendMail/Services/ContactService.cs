using AutoMapper;
using SendMail.Models;
using SendMail.Repository;

namespace SendMail.Services;

public class ContactService : IContactService
{
    private readonly ICsvService _csvService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ContactService(ICsvService csvService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _csvService = csvService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public IEnumerable<ContactDto>? ProcessContacts(Stream fileStream)
    {
        var records = _csvService.ProcessCsv<ContactDto>(fileStream);

        return records;
    }
    
    public async Task<ContactDto?> GetContact(int id)
    {
        var contact = await _unitOfWork.Contacts.GetItem(id);

        var contactDto = _mapper.Map<ContactDto>(contact);

        return contactDto;
    }

    public async Task<Contact?> PostContact(ContactDto? contactDto)
    {
        var contactToSave = _mapper.Map<Contact>(contactDto);

        var contact = await _unitOfWork.Contacts.AddItem(contactToSave);

        return contact;
    }
}