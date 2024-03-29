using System.ComponentModel.DataAnnotations;

namespace ConversionHive.Entities;

public class MailConfig
{
    [Key]
    public int Id { get; set; }
    [Required]
    [Length(2, 50)]
    public string SenderEmail { get; set; }
    [Required]
    [Length(2, 50)]
    public string Password { get; set; }
    [Required]
    [Length(2, 50)]
    public string Host { get; set; }
    [Required]
    public short Port { get; set; }

    [Required]
    public int CompanyId { get; set; }
    public Company? Company { get; set; }
    
    [Required]
    public int UserId { get; set; }
    public User? User { get; set; }
}