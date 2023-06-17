namespace SendMail.Repository;

public interface IUnitOfWork : IDisposable
{
    public IMailRepository Mails { get; }
    public IContactRepository Contacts { get; }
    Task SaveChangesAsync();
}
