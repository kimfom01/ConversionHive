using SendMail.Data;
using SendMail.Models.ContactModels;

namespace SendMail.Repository.Implementations;

public class ContactRepository : Repository<Contact>, IContactRepository
{
    public ContactRepository(SendMailDbContext context) : base(context)
    {
    }
}