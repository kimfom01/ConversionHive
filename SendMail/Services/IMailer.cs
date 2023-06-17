using SendMail.Models;

namespace SendMail.Services;

public interface IMailer
{
    Task<bool> SendMail(Mail mail);
}