using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ConversionHive.Dtos.User;
using ConversionHive.Entities;
using ConversionHive.Repository;

namespace ConversionHive.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly IJwtProcessor _jwtProcessor;

    public AuthService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IConfiguration configuration,
        IJwtProcessor jwtProcessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _configuration = configuration;
        _jwtProcessor = jwtProcessor;
    }

    public async Task<UserDto?> GetUser(string authorization)
    {
        var claim = _jwtProcessor.ExtractClaimFromJwt(authorization, "Id");

        var id = int.Parse(claim.Value);

        var user = await _unitOfWork.Users.GetItem(id);

        var userDto = _mapper.Map<UserDto>(user);

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
            .GetBytes(_configuration.GetValue<string>("Jwt:Key") ?? 
                      throw new Exception("Jwt security key not found")));

        var issuer = _configuration.GetValue<string>("Jwt:Issuer") ?? 
                     throw new Exception("Jwt issuer key not found");

        var audience = _configuration.GetValue<string>("Jwt:Audience") ?? 
                       throw new Exception("Jwt audience key not found");

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