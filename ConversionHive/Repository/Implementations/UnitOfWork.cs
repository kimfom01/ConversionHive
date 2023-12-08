using ConversionHive.Data;

namespace ConversionHive.Repository.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly SendMailDbContext _context;

    public UnitOfWork(SendMailDbContext context)
    {
        Mails = new MailRepository(context);
        Contacts = new ContactRepository(context);
        Users = new UserRepository(context);
        _context = context;
    }

    public IMailRepository Mails { get; }
    public IContactRepository Contacts { get; }
    public IUserRepository Users { get; }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
