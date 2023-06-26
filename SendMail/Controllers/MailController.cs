using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SendMail.Models;
using SendMail.Repository;
using SendMail.Services;

namespace SendMail.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MailController : ControllerBase
{
    private readonly IMailer _mailer;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MailController(IMailer mailer, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mailer = mailer;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> SendMail(MailDto sendMailDto)
    {
        var mailToSend = _mapper.Map<Mail>(sendMailDto);

        var sendResponse = await _mailer.SendMail(mailToSend);

        if (!sendResponse)
        {
            return BadRequest();
        }

        var mail = await _unitOfWork.Mails.AddItem(mailToSend);
        await _unitOfWork.SaveChangesAsync();

        // return Ok("Email Successfully Sent!");
        return CreatedAtAction(nameof(GetSavedMail), new { id = mail.Id }, mail);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetSavedMail(int id)
    {
        var mail = await _unitOfWork.Mails.GetItem(id);

        if (mail is null)
        {
            return NotFound();
        }

        var mailDto = _mapper.Map<MailDto>(mail);

        return Ok(mailDto);
    }
}