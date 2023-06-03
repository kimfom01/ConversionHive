using Microsoft.AspNetCore.Mvc;
using SendMail.Models;
using SendMail.Repository;
using SendMail.Services;

namespace SendMail.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MailController : ControllerBase
{
    private readonly IMailer _mailer;
    private readonly IMailRepository _mailRepository;

    public MailController(IMailer mailer, IMailRepository mailRepository)
    {
        _mailer = mailer;
        _mailRepository = mailRepository;
    }

    [HttpPost]
    public async Task<IActionResult> SendMail(SendMailDto sendMail)
    {
        var mailToSend = MapToMailObject(sendMail);
        
        var sendResponse = await _mailer.SendMail(mailToSend);

        if (sendResponse is null || !sendResponse.Successful)
        {
            return BadRequest(sendResponse?.ErrorMessages);
        }

        var mail = await _mailRepository.AddMail(mailToSend);

        // return Ok("Email Successfully Sent!");
        return CreatedAtAction(nameof(GetSavedMail), new {id = mail.Id}, mail);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSavedMail(int id)
    {
        var mail  = await _mailRepository.GetMail(id);

        return Ok(mail);
    }

    private Mail MapToMailObject(SendMailDto sendMailDto)
    {
        var mail = new Mail
        {
            Name = sendMailDto.Name,
            Email = sendMailDto.Email,
            RecipientEmail = sendMailDto.RecipientEmail,
            Body = sendMailDto.Body
        };
        
        if (sendMailDto.Subject is not null)
        {
            mail.Subject = sendMailDto.Subject;
        }

        return mail;
    }
}