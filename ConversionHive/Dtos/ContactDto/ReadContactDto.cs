namespace ConversionHive.Dtos.ContactDto;

public class ReadContactDto
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string EmailAddress { get; set; }
    public int CompanyId { get; set; }
}