namespace ConversionHive.Repository;

public interface IUnitOfWork : IDisposable
{
    public IMailRepository Mails { get; }
    public IContactRepository Contacts { get; }
    public IUserRepository Users { get; }

    Task SaveChangesAsync();
}
