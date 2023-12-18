using ConversionHive.Dtos.UserDto;
using ConversionHive.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ConversionHive.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("fixed-by-ip")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService, IConfiguration configuration)
    {
        _authService = authService;
    }

    [HttpGet]
    [Authorize(Roles = "CompanyAdmin, SystemAdmin")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(403)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> GetUser([FromHeader] string authorization)
    {
        var userDto = await _authService.GetUser(authorization);

        if (userDto is null)
        {
            return NotFound();
        }

        return Ok(userDto);
    }

    [HttpPost("register")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegisterDto userRegisterDto)
    {
        var userExists = await _authService.CheckUserExists(userRegisterDto.EmailAddress);

        if (userExists)
        {
            return BadRequest("User already exists");
        }

        var registeredUser = await _authService.RegisterUser(userRegisterDto);

        return CreatedAtAction(nameof(GetUser), new { id = registeredUser.Id }, registeredUser);
    }

    [HttpPost("login")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> LoginUser([FromBody] UserLoginDto userLoginDto)
    {
        var user = await _authService.GetUser(userLoginDto);

        if (user is null)
        {
            return NotFound("User does not exist");
        }

        if (!_authService.VerifyUser(userLoginDto.Password, user.PasswordHash))
        {
            return BadRequest("Username or password is wrong");
        }

        var token = _authService.GetToken(user);

        return Ok(new { token });
    }
}