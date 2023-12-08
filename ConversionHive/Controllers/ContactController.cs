using ConversionHive.Dtos.ContactDto;
using ConversionHive.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConversionHive.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Basic, Admin")]
[ProducesResponseType(401)]
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
    public async Task<IActionResult> PostContact(CreateContactDto? contactDto)
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
    public async Task<IActionResult> PostMultipleContacts([FromHeader] string authorization, [FromForm] IFormFile file)
    {
        var stream = file.OpenReadStream();

        var contacts = await _contactService.ProcessContacts(authorization, stream);

        if (contacts is null)
        {
            return BadRequest();
        }

        return Ok(contacts);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(401)]
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