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

    public async Task<List<ReadContactDto>>
        PostContactsCsv(string authorization, Stream fileStream)
    {
        // TODO: Create enum to hold claim types for different entities??
        var userIdClaim = _jwtProcessor.ExtractClaimFromJwt(authorization, "Id");

        var userId = int.Parse(userIdClaim.Value);

        var creatContactCsvDtos =
            _csvService.ProcessCsv<CreateContactCsvDto>(fileStream);

        if (creatContactCsvDtos is null)
        {
            return new List<ReadContactDto>();
        }

        var contacts = _mapper.Map<List<Contact>>(creatContactCsvDtos);

        contacts.ForEach(con => con.UserId = userId);

        await _unitOfWork.Contacts.AddItems(contacts);
        await _unitOfWork.SaveChangesAsync();

        var createContactResponseDtos =
            _mapper.Map<List<ReadContactDto>>(contacts);

        return createContactResponseDtos;
    }

    public async Task<ReadContactDto?> GetContact(string authorization, int contactId)
    {
        var userIdClaim = _jwtProcessor.ExtractClaimFromJwt(authorization, "Id");

        var userId = int.Parse(userIdClaim.Value);

        var contact = await _unitOfWork.Contacts.GetItem(con =>
            con.Id == contactId && con.UserId == userId);

        var contactDto = _mapper.Map<ReadContactDto>(contact);

        return contactDto;
    }

    public async Task<ReadContactDto?> PostContact(string authorization, CreateContactDto? contactDto)
    {
        var userIdClaim = _jwtProcessor.ExtractClaimFromJwt(authorization, "Id");

        var userId = int.Parse(userIdClaim.Value);
        
        var contactToSave = _mapper.Map<Contact>(contactDto);

        contactToSave.UserId = userId;

        var contact = await _unitOfWork.Contacts.AddItem(contactToSave);
        await _unitOfWork.SaveChangesAsync();

        var contactResponse = _mapper.Map<ReadContactDto>(contact);

        return contactResponse;
    }
}