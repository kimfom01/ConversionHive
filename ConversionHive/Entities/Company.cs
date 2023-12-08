using System.ComponentModel.DataAnnotations;

namespace ConversionHive.Entities;

public class Company
{
    [Key]
    public int Id { get; set; }
    [Required]
    [Length(2, 50)]
    public string Name { get; set; }
    [Required]
    [Length(2, 20)]
    public string Email { get; set; }
    [Required]
    [Length(2, 50)]
    public string PostalAddress { get; set; }

    [Required]
    public int UserId { get; set; }
    public User? User { get; set; }
}