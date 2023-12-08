using ConversionHive.Models.Mail;
using ConversionHive.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConversionHive.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Basic, Admin")]
public class MailController : ControllerBase
{
    private readonly IMailService _mailService;

    public MailController(IMailService mailService)
    {
        _mailService = mailService;
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> SendMail(MailDto sendMailDto)
    {
        var mail = await _mailService.SendMail(sendMailDto);

        if (mail is null)
        {
            return BadRequest();
        }

        // return Ok("Email Successfully Sent!");
        return CreatedAtAction(nameof(GetSavedMail), new { id = mail.Id }, mail);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> GetSavedMail(int id)
    {
        var mailDto = await _mailService.GetSavedMail(id);

        if (mailDto is null)
        {
            return NotFound();
        }
        
        return Ok(mailDto);
    }

    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> GetMails()
    {
        var mails = await _mailService.GetSavedMails();

        if (mails is null)
        {
            return NotFound();
        }

        return Ok(mails);
    }
}