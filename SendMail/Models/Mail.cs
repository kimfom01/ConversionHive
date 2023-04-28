namespace SendMail.Models;

public class Mail
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string RecipientEmail { get; set; } = string.Empty;
    public string? Subject { get; set; }
    public string Body { get; set; } = string.Empty;
}