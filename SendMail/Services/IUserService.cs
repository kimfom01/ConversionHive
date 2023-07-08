using SendMail.Models.User;

namespace SendMail.Services;

public interface IUserService
{
    public Task<UserDto> GetUser(int id);
    public Task<User> RegisterUser(UserRegisterDto userRegisterDto);
    public Task<string> LoginUser(UserLoginDto userLoginDto);
    public Task<bool> CheckUserExists(string emailAddress);
    public Task<User?> GetUser(UserLoginDto userLoginDto);
    public bool VerifyUser(string password, string hash);
    public string GetToken(User user);
}
