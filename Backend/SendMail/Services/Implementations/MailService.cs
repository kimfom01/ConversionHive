using AutoMapper;
using SendMail.Models.Mail;
using SendMail.Repository;

namespace SendMail.Services.Implementations;

public class MailService : IMailService
{
    private readonly IMailer _mailer;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public MailService(IMailer mailer, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mailer = mailer;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<MailDto?> GetSavedMail(int id)
    {
        var mail = await _unitOfWork.Mails.GetItem(id);

        var mailDto = _mapper.Map<MailDto>(mail);

        return mailDto;
    }

    public async Task<Mail?> SendMail(MailDto sendMailDto)
    {
        var mailToSend = _mapper.Map<Mail>(sendMailDto);

        var success = await _mailer.SendMail(mailToSend);

        if (!success)
        {
            return null;
        }

        var mail = await _unitOfWork.Mails.AddItem(mailToSend);
        await _unitOfWork.SaveChangesAsync();

        return mail;
    }
}
