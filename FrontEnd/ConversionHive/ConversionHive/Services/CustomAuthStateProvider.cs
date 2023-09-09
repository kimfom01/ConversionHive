using Blazored.LocalStorage;
using ConversionHive.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace ConversionHive.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorageService;
    private readonly HttpClient _client;

    public CustomAuthStateProvider(ILocalStorageService localStorageService, HttpClient client)
    {
        _localStorageService = localStorageService;
        _client = client;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var state = new AuthenticationState(new ClaimsPrincipal());

        string authToken = await _localStorageService.GetItemAsync<string>("Token");

        if (!string.IsNullOrEmpty(authToken))
        {
            var userId = ExtractIdFromJwt(authToken);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            var userResponse = await _client.GetAsync($"User/{userId}");

            if (userResponse.IsSuccessStatusCode)
            {
                var stream = await userResponse.Content.ReadAsStreamAsync();
                var userModel = await JsonSerializer.DeserializeAsync<UserResponseModel>(stream);

                await _localStorageService.SetItemAsync("Id", userModel?.Id);
                await _localStorageService.SetItemAsync("FirstName", userModel?.FirstName);
                await _localStorageService.SetItemAsync("LastName", userModel?.LastName);
                await _localStorageService.SetItemAsync("EmailAddress", userModel?.EmailAddress);

                if (userModel is not null)
                {
                    var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, userModel.EmailAddress) }, "email address");

                    state = new AuthenticationState(new ClaimsPrincipal(identity));
                }
            }
        }

        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }

    private int ExtractIdFromJwt(string authorization)
    {
        var jwt = authorization.Replace("Bearer ", string.Empty);

        var handler = new JwtSecurityTokenHandler();

        var token = handler.ReadJwtToken(jwt);

        int id = int.Parse(token.Claims.First(cl => cl.Type == "Id").Value);

        return id;
    }
}
