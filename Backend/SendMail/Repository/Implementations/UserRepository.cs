using SendMail.Data;
using SendMail.Models.UserModels;

namespace SendMail.Repository.Implementations;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(SendMailDbContext context) : base(context)
    {
    }
}
