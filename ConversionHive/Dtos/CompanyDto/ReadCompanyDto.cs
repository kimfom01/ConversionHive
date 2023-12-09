namespace ConversionHive.Dtos.CompanyDto;

public class ReadCompanyDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public string? PostalAddress { get; set; }
    public int UserId { get; set; }
}