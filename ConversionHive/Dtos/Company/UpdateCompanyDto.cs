namespace ConversionHive.Dtos.Company;

public class UpdateCompanyDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public string? PostalAddress { get; set; }
}