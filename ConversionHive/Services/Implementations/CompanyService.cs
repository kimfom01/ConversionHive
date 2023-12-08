using System.Security.Claims;
using AutoMapper;
using ConversionHive.Dtos.Company;
using ConversionHive.Entities;
using ConversionHive.Repository;

namespace ConversionHive.Services.Implementations;

public class CompanyService : ICompanyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtProcessor _jwtProcessor;
    private readonly IMapper _mapper;

    public CompanyService(
        IUnitOfWork unitOfWork,
        IJwtProcessor jwtProcessor,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _jwtProcessor = jwtProcessor;
        _mapper = mapper;
    }

    public async Task<ReadCompanyDto> GetCompany(string authorization, int companyId)
    {
        var userId = GetIdFromClaim(authorization, "Id");

        var company = await _unitOfWork.Companies.GetItem(com =>
            com.UserId == userId);

        if (company is null)
        {
            throw new Exception($"Company with id {companyId} not found");
        }

        var companyDto = _mapper.Map<ReadCompanyDto>(company);

        return companyDto;
    }

    public async Task<ReadCompanyDto> CreateCompany(string authorization, CreateCompanyDto createCompanyDto)
    {
        var userId = GetIdFromClaim(authorization, "Id");

        var company = _mapper.Map<Company>(createCompanyDto);

        company.UserId = userId;

        var created = await _unitOfWork.Companies.AddItem(company);
        await _unitOfWork.SaveChangesAsync();

        var returnValue = _mapper.Map<ReadCompanyDto>(created);

        return returnValue;
    }

    public async Task UpdateCompany(string authorization, UpdateCompanyDto updateCompanyDto)
    {
        var userId = GetIdFromClaim(authorization, "Id");

        var company = await _unitOfWork.Companies.GetItem(com =>
            com.Id == updateCompanyDto.Id && com.UserId == userId);

        if (company is null)
        {
            throw new Exception($"Company with id {updateCompanyDto.Id} not found");
        }

        _mapper.Map(updateCompanyDto, company);

        await _unitOfWork.Companies.Update(company);
        await _unitOfWork.SaveChangesAsync();
    }

    private int GetIdFromClaim(string authorization, string claimType)
    {
        var claim = _jwtProcessor.ExtractClaimFromJwt(authorization, claimType);

        return int.Parse(claim.Value);
    }
}