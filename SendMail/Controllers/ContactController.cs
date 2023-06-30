using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SendMail.Models;
using SendMail.Services;

namespace SendMail.Controllers;

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
    public async Task<IActionResult> PostContact(ContactDto? contactDto)
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
    public async Task<IActionResult> PostMultipleContacts([FromForm] IFormFileCollection file)
    {
        var stream = file[0].OpenReadStream();
        
        var contacts = await _contactService.ProcessContacts(stream);

        if (contacts is null)
        {
            return BadRequest();
        }
        
        return Ok(contacts);
    }

    [HttpGet("{id}")]
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