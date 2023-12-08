using ConversionHive.Data;
using ConversionHive.Models.ContactModels;

namespace ConversionHive.Repository.Implementations;

public class ContactRepository : Repository<Contact>, IContactRepository
{
    public ContactRepository(SendMailDbContext context) : base(context)
    {
    }
}