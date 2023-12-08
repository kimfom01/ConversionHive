using System.ComponentModel.DataAnnotations;

namespace ConversionHive.Entities;

public class Contact
{
    [Key]
    public int Id { get; set; }
    [Length(2, 50)]
    public string? FirstName { get; set; }
    [Length(2, 50)]
    public string? LastName { get; set; }
    [Required]
    [Length(2, 50)]
    public string EmailAddress { get; set; }

    [Required]
    public int CompanyId { get; set; }
    public Company? Company { get; set; }
}