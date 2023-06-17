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
    private readonly IUnitOfWork _unitOfWork;

    public MailController(IMailer mailer, IUnitOfWork unitOfWork)
    {
        _mailer = mailer;
        _unitOfWork = unitOfWork;
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

        var mail = await _unitOfWork.Mails.AddItem(mailToSend);
        await _unitOfWork.SaveChangesAsync();

        // return Ok("Email Successfully Sent!");
        return CreatedAtAction(nameof(GetSavedMail), new { id = mail.Id }, mail);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSavedMail(int id)
    {
        var mail = await _unitOfWork.Mails.GetItem(id);

        if (mail is null)
        {
            return NotFound();
        }

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