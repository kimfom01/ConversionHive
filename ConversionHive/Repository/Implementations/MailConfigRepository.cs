using ConversionHive.Data;
using ConversionHive.Entities;

namespace ConversionHive.Repository.Implementations;

public class MailConfigRepository : Repository<MailConfig>, IMailConfigRepository
{
    public MailConfigRepository(SendMailDbContext context) : base(context)
    {
    }
}