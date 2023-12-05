using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SendMail.Models.UserModels;
using SendMail.Services;

namespace SendMail.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
    }

    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [Authorize(Roles = "Basic, Admin")]
    [ProducesResponseType(401)]
    public async Task<IActionResult> GetUser([FromHeader] string authorization)
    {
        var userDto = await _userService.GetUser(authorization);

        if (userDto is null)
        {
            return NotFound();
        }

        return Ok(userDto);
    }

    [HttpPost("register")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> RegisterUser(UserRegisterDto? userRegisterDto)
    {
        if (userRegisterDto is null)
        {
            return BadRequest("Invalid details");
        }

        var userExists = await _userService.CheckUserExists(userRegisterDto.EmailAddress);

        if (userExists)
        {
            return BadRequest("User already exists");
        }

        var registeredUser = await _userService.RegisterUser(userRegisterDto);

        return CreatedAtAction(nameof(GetUser), new { id = registeredUser.Id }, registeredUser);
    }

    [HttpPost("login")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> LoginUser(UserLoginDto? userLoginDto)
    {
        if (userLoginDto is null)
        {
            return BadRequest("Invalid details");
        }

        var user = await _userService.GetUser(userLoginDto);

        if (user is null)
        {
            return NotFound("User does not exist");
        }

        if (!_userService.VerifyUser(userLoginDto.Password, user.PasswordHash))
        {
            return BadRequest("Username or password is wrong");
        }

        var token = _userService.GetToken(user);

        return Ok(token);
    }
}
