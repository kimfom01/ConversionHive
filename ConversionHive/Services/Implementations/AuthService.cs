using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ConversionHive.Dtos.UserDto;
using ConversionHive.Entities;
using ConversionHive.Repository;

namespace ConversionHive.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly IJwtProcessor _jwtProcessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public AuthService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IConfiguration configuration,
        IJwtProcessor jwtProcessor,
        IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _configuration = configuration;
        _jwtProcessor = jwtProcessor;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<ReadUserDto?> GetUser(string authorization)
    {
        var userId = _jwtProcessor.GetIdFromJwt(authorization);

        var user = await _unitOfWork.Users.GetItem(userId);

        var userDto = _mapper.Map<ReadUserDto>(user);

        return userDto;
    }

    public async Task<User> RegisterUser(UserRegisterDto userRegisterDto)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(userRegisterDto.Password);

        var user = _mapper.Map<User>(userRegisterDto);

        user.PasswordHash = passwordHash;
        user.Role = RolesEnum.CompanyAdmin.ToString();

        var registeredUser = await _unitOfWork.Users.AddItem(user);
        await _unitOfWork.SaveChangesAsync();

        return registeredUser;
    }

    public async Task<bool> CheckUserExists(string emailAddress)
    {
        var user = await _unitOfWork
            .Users.GetItem(user =>
                user.EmailAddress == emailAddress);

        return user is not null;
    }

    public async Task<User?> GetUser(UserLoginDto userLoginDto)
    {
        var user = await _unitOfWork
            .Users.GetItem(user =>
                user.EmailAddress == userLoginDto.EmailAddress);

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

        var key = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(EnvironmentConfigHelper.GetKey(_configuration, _webHostEnvironment)));

        var issuer = EnvironmentConfigHelper.GetIssuer(_configuration, _webHostEnvironment);

        var audience = EnvironmentConfigHelper.GetAudience(_configuration, _webHostEnvironment);

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddMinutes(14),
            signingCredentials: credentials, issuer: issuer, audience: audience);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    public string GetToken(User user)
    {
        var jwtToken = CreateToken(user, user.Role);

        return jwtToken;
    }
}