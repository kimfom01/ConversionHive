using SendMail.Data;
using SendMail.Models.Contact;

namespace SendMail.Repository.Implementations;

public class ContactRepository : Repository<Contact>, IContactRepository
{
    public ContactRepository(SendMailDbContext context) : base(context)
    {
    }
}