using System.ComponentModel.DataAnnotations;

namespace ConversionHive.Entities;

public class User
{
    [Key]
    public int Id { get; set; }
    [Required]
    [Length(2, 50)]
    public string FirstName { get; set; }
    [Required]
    [Length(2, 50)]
    public string LastName { get; set; }
    [Required]
    [Length(2, 50)]
    public string EmailAddress { get; set; }
    [Required]
    public string PasswordHash { get; set; }
    public string Role { get; set; }
}
