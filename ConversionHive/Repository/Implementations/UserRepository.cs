using ConversionHive.Data;
using ConversionHive.Entities;

namespace ConversionHive.Repository.Implementations;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(SendMailDbContext context) : base(context)
    {
    }
}
