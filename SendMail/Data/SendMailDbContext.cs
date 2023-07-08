using Microsoft.EntityFrameworkCore;
using SendMail.Models;
using SendMail.Models.Contact;
using SendMail.Models.User;

namespace SendMail.Data;

public class SendMailDbContext : DbContext
{
    public DbSet<Mail> Mails { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<User> Users { get; set; }

    public SendMailDbContext(DbContextOptions<SendMailDbContext> options) : base(options)
    {
    }
}