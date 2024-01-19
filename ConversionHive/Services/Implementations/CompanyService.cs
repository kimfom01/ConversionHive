using AutoMapper;
using ConversionHive.Dtos.CompanyDto;
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
        var userId = _jwtProcessor.GetIdFromJwt(authorization);

        var company = await _unitOfWork.Companies.GetItem(com =>
            com.UserId == userId && com.Id == companyId);

        if (company is null)
        {
            throw new Exception($"Company with id {companyId} not found");
        }

        var companyDto = _mapper.Map<ReadCompanyDto>(company);

        return companyDto;
    }

    public async Task<ReadCompanyDto> CreateCompany(string authorization, CreateCompanyDto createCompanyDto)
    {
        var userId = _jwtProcessor.GetIdFromJwt(authorization);

        var company = _mapper.Map<Company>(createCompanyDto);

        company.UserId = userId;

        var created = await _unitOfWork.Companies.AddItem(company);
        await _unitOfWork.SaveChangesAsync();

        var returnValue = _mapper.Map<ReadCompanyDto>(created);

        return returnValue;
    }

    public async Task UpdateCompany(string authorization, UpdateCompanyDto updateCompanyDto)
    {
        var userId = _jwtProcessor.GetIdFromJwt(authorization);

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

    public async Task UpdateCompanyName(string authorization, UpdateCompanyNameDto updateCompanyNameDto)
    {
        var userId = _jwtProcessor.GetIdFromJwt(authorization);

        var company = await _unitOfWork.Companies.GetItem(com =>
            com.Id == updateCompanyNameDto.Id && com.UserId == userId);

        if (company is null)
        {
            throw new Exception($"Company with id {updateCompanyNameDto.Id} not found");
        }

        _mapper.Map(updateCompanyNameDto, company);

        await _unitOfWork.Companies.Update(company);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateCompanyEmail(string authorization, UpdateCompanyEmailDto updateCompanyEmailDto)
    {
        var userId = _jwtProcessor.GetIdFromJwt(authorization);

        var company = await _unitOfWork.Companies.GetItem(com =>
            com.Id == updateCompanyEmailDto.Id && com.UserId == userId);

        if (company is null)
        {
            throw new Exception($"Company with id {updateCompanyEmailDto.Id} not found");
        }

        _mapper.Map(updateCompanyEmailDto, company);

        await _unitOfWork.Companies.Update(company);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateCompanyPostalAddress(string authorization,
        UpdateCompanyPostalAddressDto updateCompanyPostalAddressDto)
    {
        var userId = _jwtProcessor.GetIdFromJwt(authorization);

        var company = await _unitOfWork.Companies.GetItem(com =>
            com.Id == updateCompanyPostalAddressDto.Id && com.UserId == userId);

        if (company is null)
        {
            throw new Exception($"Company with id {updateCompanyPostalAddressDto.Id} not found");
        }

        _mapper.Map(updateCompanyPostalAddressDto, company);

        await _unitOfWork.Companies.Update(company);
        await _unitOfWork.SaveChangesAsync();
    }
}