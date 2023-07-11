using System.Net;
using System.Net.Mail;
using System.Security;
using FluentEmail.Core;
using FluentEmail.Smtp;
using SendMail.Models.Mail;

namespace SendMail.Services.Implementations;

public class NetworkEmailSender : IMailer
{
    private readonly string _username;
    private readonly SecureString _password;
    private readonly string _host;
    private readonly int _port;
    private readonly SmtpClient _client;

    public NetworkEmailSender(IConfiguration configuration)
    {
        _username = configuration.GetSection("EmailCredentials:username").Value
                    ?? Environment.GetEnvironmentVariable("USERNAME")!;
        _password = GetSecurePassword(configuration.GetSection("EmailCredentials:password").Value
                    ?? Environment.GetEnvironmentVariable("PASSWORD")!);
        _host = configuration.GetSection("EmailCredentials:host").Value
                    ?? Environment.GetEnvironmentVariable("HOST")!;
        var parsePortResult = int.TryParse(configuration.GetSection("EmailCredentials:port").Value
                    ?? Environment.GetEnvironmentVariable("PORT"), out _port);

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

    private SecureString GetSecurePassword(string password)
    {
        var secureString = new SecureString();

        foreach (var character in password)
        {
            secureString.AppendChar(character);
        }

        return secureString;
    }

    public async Task<bool> SendMail(Mail mail)
    {
        var sender = new SmtpSender(() => _client);

        Email.DefaultSender = sender;

        var sendResponse = await Email
            .From(mail.Sender, mail.Name)
            .To(mail.Receiver)
            .Subject(mail.Subject)
            .Body(mail.Body)
            .SendAsync();

        return sendResponse.Successful;
    }
}