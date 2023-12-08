using ConversionHive.Models.UserModels;

namespace ConversionHive.Services;

public interface IAuthService
{
    public Task<UserDto?> GetUser(string authorization);
    public Task<User> RegisterUser(UserRegisterDto userRegisterDto);
    public Task<bool> CheckUserExists(string emailAddress);
    public Task<User?> GetUser(UserLoginDto userLoginDto);
    public bool VerifyUser(string password, string hash);
    public string GetToken(User user);
}
