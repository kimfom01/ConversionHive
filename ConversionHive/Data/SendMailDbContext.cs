using ConversionHive.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConversionHive.Data;

public class SendMailDbContext : DbContext
{
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<MailConfig> MailConfigs { get; set; }

    public SendMailDbContext(DbContextOptions<SendMailDbContext> options) : base(options)
    {
    }
}