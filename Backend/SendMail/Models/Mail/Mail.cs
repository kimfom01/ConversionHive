using SendMail.Models.ContactModels;

namespace SendMail.Models.Mail;

public class Mail
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Sender { get; set; } = string.Empty;
    //public string Receiver { get; set; } = string.Empty;
    public string? Subject { get; set; }
    public string Body { get; set; } = string.Empty;

    public List<ReceiverContactDto>? Receivers { get; set; }
}