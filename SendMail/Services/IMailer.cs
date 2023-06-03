using FluentEmail.Core.Models;
using SendMail.Models;

namespace SendMail.Services;

public interface IMailer
{
    Task<SendResponse?> SendMail(Mail mail);
}