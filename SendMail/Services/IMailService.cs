using SendMail.Models;

namespace SendMail.Services;

public interface IMailService
{
    Task<Mail?> SendMail(MailDto sendMailDto);
    Task<MailDto?> GetSavedMail(int id);
}
