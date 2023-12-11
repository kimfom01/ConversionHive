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
    public async Task<ActionResult<ReadContactDto>> PostContact([FromHeader] string authorization, [FromBody] CreateContactDto? contactDto)
    {
        var contact = await _contactService.PostContact(authorization, contactDto);

        return CreatedAtAction(nameof(GetContact), new { contactId = contact.Id }, contact);
    }

    [HttpPost("csv")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<ReadContactDto>> PostMultipleContacts([FromHeader] string authorization,
        [FromForm] IFormFile file)
    {
        try
        {
            var stream = file.OpenReadStream();

            var contacts = await _contactService.PostContactsCsv(authorization, stream);

            if (contacts.Count == 0)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetContact), new { contactId = contacts.First().Id }, contacts);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{contactId:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<ReadContactDto>> GetContact([FromHeader] string authorization, [FromRoute] int contactId)
    {
        var contactDto = await _contactService.GetContact(authorization, contactId);

        if (contactDto is null)
        {
            return NotFound();
        }

        return Ok(contactDto);
    }
}