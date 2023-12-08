using System.Net;
using System.Net.Mail;
using System.Security;
using ConversionHive.Models;
using FluentEmail.Core;
using FluentEmail.Smtp;

namespace ConversionHive.Services.Implementations;

public class MailService : IMailService
{
    private readonly string _username;
    private readonly SecureString _password;
    private readonly string _host;
    private readonly int _port;
    private readonly SmtpClient _client;

    public MailService(IConfiguration configuration)
    {
        _username = configuration.GetSection("EmailCredentials:username").Value
                    ?? Environment.GetEnvironmentVariable("USERNAME") ??
                    throw new Exception("Username not found");
        _password = GetSecurePassword(configuration.GetSection("EmailCredentials:password").Value
                                      ?? Environment.GetEnvironmentVariable("PASSWORD") ??
                                      throw new Exception("Password not found"));
        _host = configuration.GetSection("EmailCredentials:host").Value
                ?? Environment.GetEnvironmentVariable("HOST") ??
                throw new Exception("Host not found");
        var parsePortResult = int.TryParse(configuration.GetSection("EmailCredentials:port").Value
                                           ?? Environment.GetEnvironmentVariable("PORT") ??
                                           throw new Exception("Port not found"), out _port);
        if (!parsePortResult)
        {
            throw new Exception("Invalid port value");
        }

        _client = GetClient();
    }

    private SmtpClient GetClient()
    {
        var client = new SmtpClient(_host, _port)
        {
            Credentials = new NetworkCredential(_username, _password),
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network
        };

        return client;
    }

    public async Task SendMail(Mail mail)
    {
        var sender = new SmtpSender(() => _client);

        Email.DefaultSender = sender;

        var sendResponse = await Email
            .From("Sender", "Sender name")
            .To(mail.RecipientEmail)
            .Subject(mail.Subject)
            .Body(mail.Body)
            .SendAsync();

        if (!sendResponse.Successful)
        {
            throw new Exception("Failed to send email");
        }
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
}