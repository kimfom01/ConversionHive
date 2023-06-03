using SendMail.Models;

namespace SendMail.Repository;

public interface IMailRepository
{
    Task<Mail> AddMail(Mail mail);
    Task<Mail?> GetMail(int id);
}