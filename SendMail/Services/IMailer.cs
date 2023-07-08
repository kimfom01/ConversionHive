using SendMail.Models.Mail;

namespace SendMail.Services;

public interface IMailer
{
    Task<bool> SendMail(Mail mail);
}