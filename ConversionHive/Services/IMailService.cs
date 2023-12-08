using ConversionHive.Models.Mail;

namespace ConversionHive.Services;

public interface IMailService
{
    Task<Mail?> SendMail(MailDto sendMailDto);
    Task<MailDto?> GetSavedMail(int id);
    Task<IEnumerable<MailDto>?> GetSavedMails();
}
