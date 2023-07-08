using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using SendMail.Models.UserModels;
using SendMail.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SendMail.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<UserDto> GetUser(int id)
    {
        var user = await _unitOfWork.Users.GetItem(id);

        var userDto = _mapper.Map<UserDto>(user);

        return userDto;
    }

    public Task<string> LoginUser(UserLoginDto userLoginDto)
    {
        throw new NotImplementedException();
    }

    public async Task<User> RegisterUser(UserRegisterDto userRegisterDto)
    {
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(userRegisterDto.Password);

        var user = _mapper.Map<User>(userRegisterDto);

        user.PasswordHash = passwordHash;
        user.Role = RolesEnum.Basic.ToString();

        var registeredUser = await _unitOfWork.Users.AddItem(user);
        await _unitOfWork.SaveChangesAsync();

        return user;
    }

    public async Task<bool> CheckUserExists(string emailAddress)
    {
        var user = await _unitOfWork.Users.GetItem(user => user.EmailAddress == emailAddress);

        if (user is null)
        {
            return false;
        }

        return true;
    }

    public async Task<User?> GetUser(UserLoginDto userLoginDto)
    {
        var user = await _unitOfWork.Users.GetItem(user => user.EmailAddress == userLoginDto.EmailAddress);

        return user;
    }

    public bool VerifyUser(string password, string hash)
    {
        var verified = BCrypt.Net.BCrypt.Verify(password, hash);

        return verified;
    }

    private string CreateToken(User user, string role)
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, user.EmailAddress),
            new Claim(ClaimTypes.Role, role),
            new Claim("Id", user.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Key")!));

        var issuer = _configuration.GetValue<string>("Jwt:Issuer");

        var audience = _configuration.GetValue<string>("Jwt:Audience");

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(14), signingCredentials: credentials, issuer: issuer, audience: audience);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    public string GetToken(User user)
    {
        var jwtToken = CreateToken(user, user.Role);

        return jwtToken;
    }
}
