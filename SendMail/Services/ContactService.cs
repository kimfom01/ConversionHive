using AutoMapper;
using SendMail.Models.Contact;
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
    
    public async Task<IEnumerable<Contact>?> ProcessContacts(Stream fileStream)
    {
        var contactDtos = _csvService.ProcessCsv<ContactDto>(fileStream);
        
        if (contactDtos is null)
        {
            return null;
        }
        
        var contacts = _mapper.Map<IEnumerable<Contact>>(contactDtos);

        await _unitOfWork.Contacts.AddItems(contacts);
        await _unitOfWork.SaveChangesAsync();

        return contacts;
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
        await _unitOfWork.SaveChangesAsync();

        return contact;
    }
}