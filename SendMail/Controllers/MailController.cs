using Microsoft.AspNetCore.Mvc;
using SendMail.Models;
using SendMail.Services;

namespace SendMail.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MailController : ControllerBase
{
    private readonly Mailer _mailer;

    public MailController(Mailer mailer)
    {
        _mailer = mailer;
    }
    
    [HttpPost]
    public async Task<IActionResult> SendMail(Mail mail)
    {
        // var sender = new SmtpSender(() => new SmtpClient("localhost")
        // {
        //     EnableSsl = true,
        //     DeliveryMethod = SmtpDeliveryMethod.Network,
        //     Port = 2525
        // });

        // Email.DefaultSender = sender;
        //
        // var sendResponse = await Email
        //     .From(mail.Email, mail.Name)
        //     .To("myemail@mydomain.com")
        //     .Subject(mail.Subject)
        //     .Body(mail.Body)
        //     .SendAsync();

        var sendResponse = await _mailer.SendMail(mail);

        if (!sendResponse.Successful)
        {
            return BadRequest(sendResponse.ErrorMessages);
        }

        return Ok("Email Successfully Sent!");
    }
}