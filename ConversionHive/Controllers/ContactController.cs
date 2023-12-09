using ConversionHive.Dtos.ContactDto;
using ConversionHive.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConversionHive.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "CompanyAdmin, SystemAdmin")]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<ReadContactDto>> PostContact([FromBody] CreateContactDto? contactDto)
    {
        if (contactDto is null)
        {
            return BadRequest();
        }

        var contact = await _contactService.PostContact(contactDto);

        return CreatedAtAction(nameof(GetContact), new { id = contact!.Id }, contact);
    }

    [HttpPost("csv")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<ReadContactDto>> PostMultipleContacts([FromHeader] string authorization, [FromForm] IFormFile file)
    {
        var stream = file.OpenReadStream();

        var contacts = await _contactService.PostContactsCsv(authorization, stream);

        if (contacts.Count == 0)
        {
            return BadRequest();
        }
        
        return CreatedAtAction(nameof(GetContact), new { id = contacts.First().Id }, contacts);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<ReadContactDto>> GetContact([FromRoute] int id)
    {
        var contactDto = await _contactService.GetContact(id);

        if (contactDto is null)
        {
            return NotFound();
        }

        return Ok(contactDto);
    }
}