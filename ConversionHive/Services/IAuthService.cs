﻿using ConversionHive.Dtos.UserDto;
using ConversionHive.Entities;

namespace ConversionHive.Services;

public interface IAuthService
{
    public Task<ReadUserDto?> GetUser(string authorization);
    public Task<User> RegisterUser(UserRegisterDto userRegisterDto);
    public Task<bool> CheckUserExists(string emailAddress);
    public Task<User?> GetUser(UserLoginDto userLoginDto);
    public bool VerifyUser(string password, string hash);
    public string GetToken(User user);
}
