using System.Text.Json.Serialization;

namespace ConversionHive.Models;

public class RegisterModel
{
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }
    [JsonPropertyName("lastName")]
    public string LastName { get; set; }
    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; set; }
    [JsonPropertyName("password")]
    public string Password { get; set; }
}
