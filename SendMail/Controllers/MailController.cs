using Microsoft.AspNetCore.Mvc;
using SendMail.Models;
using SendMail.Services;

namespace SendMail.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MailController : ControllerBase
{
    private readonly IMailer _mailer;

    public MailController(IMailer mailer)
    {
        _mailer = mailer;
    }

    [HttpPost]
    public async Task<IActionResult> SendMail(Mail mail)
    {
        var sendResponse = await _mailer.SendMail(mail);

        if (sendResponse is null || !sendResponse.Successful)
        {
            return BadRequest(sendResponse?.ErrorMessages);
        }

        return Ok("Email Successfully Sent!");
    }
}