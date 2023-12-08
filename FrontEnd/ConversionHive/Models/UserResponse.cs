using System.Text.Json.Serialization;

namespace ConversionHive.Models;

public class UserResponseModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }
    [JsonPropertyName("lastName")]
    public string LastName { get; set; }
    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; set; }
}
