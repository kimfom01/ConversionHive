namespace ConversionHive.Models;

public class Mail
{
    public string? Subject { get; set; }
    public string Body { get; set; }
    public string RecipientEmail { get; set; }
    public string RecipientName { get; set; }
}