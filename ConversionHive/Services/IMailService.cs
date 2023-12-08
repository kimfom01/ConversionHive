using ConversionHive.Models;

namespace ConversionHive.Services;

public interface IMailService
{
    Task SendMail(Mail mail);
}
