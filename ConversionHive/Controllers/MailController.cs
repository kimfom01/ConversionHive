using ConversionHive.Models;
using ConversionHive.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConversionHive.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "CompanyAdmin, SystemAdmin")]
public class MailController : ControllerBase
{
    private readonly IMailService _mailService;

    public MailController(IMailService mailService)
    {
        _mailService = mailService;
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> SendMail(Mail mail)
    {
        try
        {
            await _mailService.SendMail(mail);

            return Ok("Email Successfully Sent!");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}