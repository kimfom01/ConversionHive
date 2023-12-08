using ConversionHive.Dtos.ContactDto;

namespace ConversionHive.Models.Mail;

public class MailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Sender { get; set; } = string.Empty;
    //public string Receiver { get; set; } = string.Empty;
    public string? Subject { get; set; }
    public string Body { get; set; } = string.Empty;

    public List<ReceiverContactDto>? Receivers { get; set; }
}