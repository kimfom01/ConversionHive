using ConversionHive.Data;
using ConversionHive.Models.UserModels;

namespace ConversionHive.Repository.Implementations;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(SendMailDbContext context) : base(context)
    {
    }
}
