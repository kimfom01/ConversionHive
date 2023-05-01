using System.Net;
using System.Net.Mail;
using System.Security;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using FluentEmail.Smtp;
using SendMail.Models;

namespace SendMail.Services;

public class Mailer
{
    private readonly string _username;
    private readonly SecureString _password;
    private readonly SmtpClient _client;

    public Mailer(IConfiguration configuration)
    {
        _username = configuration.GetSection("EmailCredentials:username").Value ??
                    Environment.GetEnvironmentVariable("USERNAME")!;
        _password = GetSecurePassword(configuration.GetSection("EmailCredentials:password").Value ??
                                      Environment.GetEnvironmentVariable("PASSWORD")!);

        _client = GetClient();
    }

    private SmtpClient GetClient()
    {
        var client = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential(_username, _password),
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network
        };

        return client;
    }

    private SecureString GetSecurePassword(string password)
    {
        var secureString = new SecureString();

        foreach (var character in password)
        {
            secureString.AppendChar(character);
        }

        return secureString;
    }

    public async Task<SendResponse> SendMail(Mail mail)
    {
        var sender = new SmtpSender(() => _client);

        Email.DefaultSender = sender;

        var sendResponse = await Email
            .From(mail.Email, mail.Name)
            .To(mail.RecipientEmail)
            .Subject(mail.Subject)
            .Body(mail.Body)
            .SendAsync();

        return sendResponse;
    }
}