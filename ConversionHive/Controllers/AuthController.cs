﻿using ConversionHive.Models.UserModels;
using ConversionHive.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConversionHive.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService, IConfiguration configuration)
    {
        _authService = authService;
    }

    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [Authorize(Roles = "Basic, Admin")]
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
    public async Task<IActionResult> RegisterUser(UserRegisterDto? userRegisterDto)
    {
        if (userRegisterDto is null)
        {
            return BadRequest("Invalid details");
        }

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
    public async Task<IActionResult> LoginUser(UserLoginDto? userLoginDto)
    {
        if (userLoginDto is null)
        {
            return BadRequest("Invalid details");
        }

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