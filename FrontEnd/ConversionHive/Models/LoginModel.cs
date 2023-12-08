using System.Text.Json.Serialization;

namespace ConversionHive.Models;

public class LoginModel
{
    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }
}
