using SendMail.Data;
using SendMail.Models;

namespace SendMail.Repository.Implementations;

public class MailRepository : Repository<Mail>, IMailRepository
{
    public MailRepository(SendMailDbContext context) : base(context)
    {
    }
}