using ConversionHive.Dtos.MailConfigDto;
using ConversionHive.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ConversionHive.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("fixed-by-ip")]
public class MailConfigController : ControllerBase
{
    private readonly IMailConfigService _mailConfigService;

    public MailConfigController(IMailConfigService mailConfigService)
    {
        _mailConfigService = mailConfigService;
    }

    [HttpGet("{companyId:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(403)]
    public async Task<ActionResult<ReadMailConfigDto>> GetMailConfig([FromHeader] string authorization,
        [FromRoute] int companyId)
    {
        try
        {
            var readMailConfigDto = await _mailConfigService.GetMailConfig(authorization, companyId);

            return Ok(readMailConfigDto);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(403)]
    public async Task<ActionResult<ReadMailConfigDto>> PostMailConfig([FromHeader] string authorization,
        [FromBody] CreateMailConfigDto createMailConfigDto)
    {
        try
        {
            var added = await _mailConfigService.PostMailConfig(authorization, createMailConfigDto);

            return CreatedAtAction(nameof(GetMailConfig), new { companyId = added.Id }, added);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> PutMailConfig([FromHeader] string authorization,
        [FromBody] UpdateMailConfigDto updateMailConfigDto)
    {
        try
        {
            await _mailConfigService.UpdateMailConfig(authorization, updateMailConfigDto);

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}