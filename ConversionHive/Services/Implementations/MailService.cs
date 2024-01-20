using System.Net;
using System.Net.Mail;
using System.Security;
using ConversionHive.Models;
using FluentEmail.Core;
using FluentEmail.Razor;
using FluentEmail.Smtp;

namespace ConversionHive.Services.Implementations;

public class MailService : IMailService
{
    private readonly IMailConfigService _mailConfigService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private string _username;
    private SecureString _password;
    private string _host;
    private int _port;
    private SmtpClient _client;

    public MailService(IMailConfigService mailConfigService, IWebHostEnvironment webHostEnvironment)
    {
        _mailConfigService = mailConfigService;
        _webHostEnvironment = webHostEnvironment;
    }

    private async Task InitConfig(string authorization, int companyId)
    {
        var mailConfig = await _mailConfigService.GetMailConfig(authorization, companyId);
        _username = mailConfig.SenderEmail;
        _password = GetSecurePassword(mailConfig.Password);
        _host = mailConfig.Host;
        _port = mailConfig.Port;
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

    // Add policy to retry sending of mails on failure  
    public async Task SendMail(string authorization, Mail mail, int companyId)
    {
        await InitConfig(authorization, companyId);

        var sender = new SmtpSender(() => _client);

        // Email.DefaultRenderer = new RazorRenderer();
        Email.DefaultSender = sender;

        // var templateFile = Path.Combine(_webHostEnvironment.WebRootPath, "FirstTemplate.cshtml");

//          Email.DefaultRenderer = new RazorRenderer();
//
          var template = """
                              Contact Email: @Model.ContactEmail
                              Contact Name: @Model.ContactName
                              Subject: @Model.Subject
                              @Model.Body
                         """;
//
//          var email = Email
//              .From("bob@hotmail.com")
//              .To("somedude@gmail.com")
//              .Subject("woo nuget")
//              .UsingTemplate(template, mail);
        
        //<!DOCTYPE html>
        // <html lang="en">
        //   <head>
        //     <meta charset="UTF-8" />
        //     <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        //     <title>Document</title>
        //   </head>
        //   <body>
        //     Hello @Model.RecipientName,
        //     <p>@Model.Body</p>
        //   </body>
        // </html>

        var sendResponse = await Email
            .From(_username)
            .To(mail.RecipientEmail)
            .Subject(mail.Subject)
            .Body(mail.Body)
            .UsingTemplate(template, mail)
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