namespace ConversionHive.Dtos.MailConfigDto;

public class ReadMailConfigDto
{
    public int Id { get; set; }
    public required string SenderEmail { get; set; }
    public required string Password { get; set; }
    public required string Host { get; set; }
    public short Port { get; set; }
    public int CompanyId { get; set; }
}