using ConversionHive.Entities;
using ConversionHive.Models.Mail;
using Microsoft.EntityFrameworkCore;

namespace ConversionHive.Data;

public class SendMailDbContext : DbContext
{
    public DbSet<Mail> Mails { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<User> Users { get; set; }

    public SendMailDbContext(DbContextOptions<SendMailDbContext> options) : base(options)
    {
    }
}