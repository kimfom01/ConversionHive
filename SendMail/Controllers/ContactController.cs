using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SendMail.Models;
using SendMail.Repository;
using SendMail.Services;

namespace SendMail.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IContactService _contactService;

    public ContactController(IUnitOfWork unitOfWork, IMapper mapper, IContactService contactService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _contactService = contactService;
    }
    
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> PostContact(ContactDto? contactDto)
    {
        if (contactDto is null)
        {
            return BadRequest();
        }
        
        var contactToSave = _mapper.Map<Contact>(contactDto);

        var contact = await _unitOfWork.Contacts.AddItem(contactToSave);

        return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
    }

    [HttpPost("csv")]
    public async Task<IActionResult> PostMultipleContacts([FromForm] IFormFileCollection file)
    {
        var stream = file[0].OpenReadStream();
        
        var contactsDtos = _contactService.ProcessContacts(stream);

        if (contactsDtos is null)
        {
            return BadRequest();
        }

        var contacts = _mapper.Map<IEnumerable<Contact>>(contactsDtos);

        await _unitOfWork.Contacts.AddItems(contacts);

        return Ok(contacts);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetContact(int id)
    {
        var contactDto = await _contactService.GetContact(id);

        if (contactDto is null)
        {
            return NotFound();
        }

        return Ok(contactDto);
    }
}