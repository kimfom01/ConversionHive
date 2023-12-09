namespace ConversionHive.Dtos.CompanyDto;

public class UpdateCompanyDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string PostalAddress { get; set; }
}