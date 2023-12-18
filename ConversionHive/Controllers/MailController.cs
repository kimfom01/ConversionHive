using ConversionHive.Models;
using ConversionHive.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ConversionHive.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "CompanyAdmin, SystemAdmin")]
[EnableRateLimiting("fixed-by-ip")]
public class MailController : ControllerBase
{
    private readonly IMailService _mailService;

    public MailController(IMailService mailService)
    {
        _mailService = mailService;
    }

    [HttpPost("{companyId:int}")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> SendMail([FromHeader] string authorization, [FromBody] Mail mail,
        [FromRoute] int companyId)
    {
        try
        {
            await _mailService.SendMail(authorization, mail, companyId);

            return Ok("Email Successfully Sent!");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}