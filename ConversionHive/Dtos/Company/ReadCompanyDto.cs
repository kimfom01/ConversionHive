namespace ConversionHive.Dtos.Company;

public class ReadCompanyDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string? PostalAddress { get; set; }
    public int UserId { get; set; }
}