using SendMail.Data;
using SendMail.Models;

namespace SendMail.Repository;

public class MailRepository : IMailRepository
{
    private readonly SendMailDbContext _dbContext;

    public MailRepository(SendMailDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Mail> AddMail(Mail mail)
    {
        var savedMail = await _dbContext.AddAsync(mail);

        return savedMail.Entity;
    }

    public async Task<Mail?> GetMail(int id)
    {
        var mail = await _dbContext.Mails.FindAsync(id);

        return mail;
    }
}