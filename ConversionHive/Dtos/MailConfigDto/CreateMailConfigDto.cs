namespace ConversionHive.Dtos.MailConfigDto;

public class CreateMailConfigDto
{
    public required string SenderEmail { get; set; }
    public required string Password { get; set; }
    public required string Server { get; set; }
    public short Port { get; set; }
    public int CompanyId { get; set; }
}