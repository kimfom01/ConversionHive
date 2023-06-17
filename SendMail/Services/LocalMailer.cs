using FluentEmail.Core;
using FluentEmail.Smtp;
using SendMail.Models;
using System.Net.Mail;

namespace SendMail.Services;

public class LocalMailer : IMailer
{
    private readonly SmtpSender _sender;
    public LocalMailer()
    {
        _sender = new SmtpSender(() => new SmtpClient("localhost")
        {
            EnableSsl = false,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Port = 25
        });
    }
    
    public async Task<bool> SendMail(Mail mail)
    {
        Email.DefaultSender = _sender;

        var sendResponse = await Email
            .From(mail.Email, mail.Name)
            .To(mail.RecipientEmail)
            .Subject(mail.Subject)
            .Body(mail.Body)
            .SendAsync();

        return sendResponse.Successful;
    }
}
