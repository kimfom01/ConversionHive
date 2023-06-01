using FluentEmail.Core;
using FluentEmail.Core.Models;
using FluentEmail.Smtp;
using SendMail.Models;
using System.Net.Mail;

namespace SendMail.Services;

public class LocalMailer : IMailer
{
    public async Task<SendResponse> SendMail(Mail mail)
    {
        var sender = new SmtpSender(() => new SmtpClient("localhost")
        {
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Port = 2525
        });

        Email.DefaultSender = sender;

        var sendResponse = await Email
            .From(mail.Email, mail.Name)
            .To("myemail@mydomain.com")
            .Subject(mail.Subject)
            .Body(mail.Body)
            .SendAsync();

        return sendResponse;
    }
}
