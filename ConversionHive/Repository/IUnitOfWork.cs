namespace ConversionHive.Repository;

public interface IUnitOfWork : IDisposable
{
    public IMailRepository Mails { get; }
    public IContactRepository Contacts { get; }
    public IUserRepository Users { get; }
    public ICompanyRepository Companies { get; set; }

    Task SaveChangesAsync();
}