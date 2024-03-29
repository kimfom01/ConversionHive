﻿using ConversionHive.Data;

namespace ConversionHive.Repository.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly SendMailDbContext _context;

    public UnitOfWork(SendMailDbContext context)
    {
        _context = context;
        Contacts = new ContactRepository(_context);
        Users = new UserRepository(_context);
        Companies = new CompanyRepository(_context);
        MailConfigs = new MailConfigRepository(_context);
    }

    public IContactRepository Contacts { get; }
    public IUserRepository Users { get; }
    public ICompanyRepository Companies { get; set; }
    public IMailConfigRepository MailConfigs { get; set; }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}