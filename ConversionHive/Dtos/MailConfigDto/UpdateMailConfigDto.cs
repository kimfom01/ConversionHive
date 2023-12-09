namespace ConversionHive.Dtos.MailConfigDto;

public class UpdateMailConfigDto
{
    public int Id { get; set; }
    public required string SenderEmail { get; set; }
    public required string Password { get; set; }
    public required string Server { get; set; }
    public short Port { get; set; }
}