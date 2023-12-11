namespace ConversionHive.Repository;

public interface IUnitOfWork : IDisposable
{
    public IContactRepository Contacts { get; }
    public IUserRepository Users { get; }
    public ICompanyRepository Companies { get; set; }
    public IMailConfigRepository MailConfigs { get; set; }

    Task SaveChangesAsync();
}