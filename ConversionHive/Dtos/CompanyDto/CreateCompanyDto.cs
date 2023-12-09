namespace ConversionHive.Dtos.CompanyDto;

public class CreateCompanyDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public string? PostalAddress { get; set; }
    public int UserId { get; set; }
}