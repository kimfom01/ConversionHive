using System.Net.Mail;
using FluentEmail.Core;
using FluentEmail.Smtp;
using Microsoft.AspNetCore.Mvc;

namespace SendMail.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MailController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SendMail()
    {
        var sender = new SmtpSender(() => new SmtpClient("localhost")
        {
            EnableSsl = false,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Port = 2525
        });

        Email.DefaultSender = sender;

        var email = await Email
            .From("sender@test.com")
            .To("receiver@test.com", "Receiver")
            .Subject("Faking")
            .Body("This is a test email")
            .SendAsync();

        if (!email.Successful)
        {
            return BadRequest(email.ErrorMessages);
        }

        return Ok("Email Successfully Sent!");
    }
}