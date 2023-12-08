using ConversionHive.Models.Mail;

namespace ConversionHive.Services;

public interface IMailer
{
    Task<bool> SendMail(Mail mail);
}