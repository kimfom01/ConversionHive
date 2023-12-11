using ConversionHive.Models;

namespace ConversionHive.Services;

public interface IMailService
{
    Task SendMail(string authorization, Mail mail, int companyId);
}
