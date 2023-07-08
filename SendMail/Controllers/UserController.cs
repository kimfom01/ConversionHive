using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SendMail.Models.User;
using SendMail.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SendMail.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public UserController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _configuration = configuration;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [Authorize(Roles = "Basic, Admin")]
    [ProducesResponseType(401)]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _unitOfWork.Users.GetItem(id);

        if (user is null)
        {
            return BadRequest();
        }

        var userDto = _mapper.Map<UserDto>(user);

        return Ok(userDto);
    }

    [HttpPost("register")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> RegisterUser(UserRegisterDto userRegisterDto)
    {
        if (userRegisterDto is null)
        {
            return BadRequest("Invalid details");
        }

        var userExists = await CheckUserExists(userRegisterDto.EmailAddress);

        if (userExists)
        {
            return BadRequest("User alreay exists");
        }

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(userRegisterDto.Password);

        var user = _mapper.Map<User>(userRegisterDto);

        user.PasswordHash = passwordHash;
        user.Role = RolesEnum.Basic.ToString();

        var registeredUser = await _unitOfWork.Users.AddItem(user);
        await _unitOfWork.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = registeredUser.Id }, registeredUser);
    }

    private async Task<bool> CheckUserExists(string emailAddress)
    {
        var user = await _unitOfWork.Users.GetItem(user => user.EmailAddress == emailAddress);

        if (user is null)
        {
            return false;
        }

        return true;
    }

    [HttpPost("login")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> LoginUser(UserLoginDto userLoginDto)
    {
        if (userLoginDto is null)
        {
            return BadRequest("Invalid details");
        }

        var user = await _unitOfWork.Users.GetItem(user => user.EmailAddress == userLoginDto.EmailAddress);

        if (user is null)
        {
            return NotFound("User does not exist");
        }

        if (!BCrypt.Net.BCrypt.Verify(userLoginDto.Password, user.PasswordHash))
        {
            return BadRequest("Username or password is wrong");
        }

        var token = CreateToken(user, user.Role);

        return Ok(token);
    }

    private string CreateToken(User user, string role)
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, user.EmailAddress),
            new Claim(ClaimTypes.Role, role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value!));

        var issuer = _configuration.GetValue<string>("Jwt:Issuer");

        var audience = _configuration.GetValue<string>("Jwt:Audience");

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(14), signingCredentials: credentials, issuer: issuer, audience: audience);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}
