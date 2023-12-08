using ConversionHive.Data;
using ConversionHive.Entities;

namespace ConversionHive.Repository.Implementations;

public class ContactRepository : Repository<Contact>, IContactRepository
{
    public ContactRepository(SendMailDbContext context) : base(context)
    {
    }
}