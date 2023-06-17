namespace SendMail.Repository;

public interface IUnitOfWork : IDisposable
{
    public IMailRepository Mails { get; }
    Task SaveChangesAsync();
}
