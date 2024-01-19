using ConversionHive.Dtos.CompanyDto;
using ConversionHive.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ConversionHive.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "CompanyAdmin, SystemAdmin")]
[EnableRateLimiting("fixed-by-ip")]
[ProducesResponseType(401)]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpGet("{companyId:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<ReadCompanyDto>> GetCompany([FromHeader] string authorization,
        [FromRoute] int companyId)
    {
        try
        {
            var company = await _companyService.GetCompany(authorization, companyId);

            return Ok(company);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<ReadCompanyDto>> PostCompany([FromHeader] string authorization,
        [FromBody] CreateCompanyDto createCompanyDto)
    {
        try
        {
            var company = await _companyService.CreateCompany(authorization, createCompanyDto);

            return CreatedAtAction(nameof(GetCompany), new { companyId = company.Id }, company);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> PutCompany([FromHeader] string authorization,
        [FromBody] UpdateCompanyDto updateCompanyDto)
    {
        try
        {
            await _companyService.UpdateCompany(authorization, updateCompanyDto);

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPut("name")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> PutCompany([FromHeader] string authorization,
        [FromBody] UpdateCompanyNameDto updateCompanyNameDto)
    {
        try
        {
            await _companyService.UpdateCompanyName(authorization, updateCompanyNameDto);

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPut("email")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> PutCompany([FromHeader] string authorization,
        [FromBody] UpdateCompanyEmailDto updateCompanyEmailDto)
    {
        try
        {
            await _companyService.UpdateCompanyEmail(authorization, updateCompanyEmailDto);

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPut("postal-address")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> PutCompany([FromHeader] string authorization,
        [FromBody] UpdateCompanyPostalAddressDto updateCompanyPostalAddressDto)
    {
        try
        {
            await _companyService.UpdateCompanyPostalAddress(authorization, updateCompanyPostalAddressDto);

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}