using SendMail.Data;
using SendMail.Models;

namespace SendMail.Repository;

public class ContactRepository: Repository<Contact>, IContactRepository
{
    public ContactRepository(SendMailDbContext context) : base(context)
    {
    }
}