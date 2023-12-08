using ConversionHive.Data;
using ConversionHive.Entities;

namespace ConversionHive.Repository.Implementations;

public class CompanyRepository : Repository<Company>, ICompanyRepository
{
    public CompanyRepository(SendMailDbContext context) : base(context)
    {
    }
}