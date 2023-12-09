using AutoMapper;
using ConversionHive.Dtos.ContactDto;
using ConversionHive.Entities;
using ConversionHive.Repository;

namespace ConversionHive.Services.Implementations;

public class ContactService : IContactService
{
    private readonly ICsvService _csvService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IJwtProcessor _jwtProcessor;

    public ContactService(
        ICsvService csvService,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IJwtProcessor jwtProcessor)
    {
        _csvService = csvService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _jwtProcessor = jwtProcessor;
    }

    public async Task<IEnumerable<CreateContactDto>?>
        PostContactsCsv(string authorization, Stream fileStream)
    {
        // TODO: Create enum to hold claim types for different entities??
        var claim = _jwtProcessor.ExtractClaimFromJwt(authorization, "Id"); 

        var id = int.Parse(claim.Value);

        var creatContactCsvDtos =
            _csvService.ProcessCsv<CreateContactCsvDto>(fileStream);

        if (creatContactCsvDtos is null)
        {
            return null;
        }

        var contacts = _mapper.Map<List<Contact>>(creatContactCsvDtos);

        contacts.ForEach(con => con.CompanyId = id);

        await _unitOfWork.Contacts.AddItems(contacts);
        await _unitOfWork.SaveChangesAsync();

        var createContactResponseDtos = 
            _mapper.Map<IEnumerable<CreateContactDto>>(contacts);

        return createContactResponseDtos;
    }

    public async Task<ReadContactDto?> GetContact(int id)
    {
        var contact = await _unitOfWork.Contacts.GetItem(id);

        var contactDto = _mapper.Map<ReadContactDto>(contact);

        return contactDto;
    }

    public async Task<ReadContactDto?> PostContact(CreateContactDto? contactDto)
    {
        var contactToSave = _mapper.Map<Contact>(contactDto);

        var contact = await _unitOfWork.Contacts.AddItem(contactToSave);
        await _unitOfWork.SaveChangesAsync();

        var contactResponse = _mapper.Map<ReadContactDto>(contact);

        return contactResponse;
    }
}