using ConversionHive.Data;
using ConversionHive.Models;

namespace ConversionHive.Repository.Implementations;

public class MailRepository : Repository<Mail>, IMailRepository
{
    public MailRepository(SendMailDbContext context) : base(context)
    {
    }
}