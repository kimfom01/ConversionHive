using ConversionHive.Dtos.CompanyDto;

namespace ConversionHive.Services;

public interface ICompanyService
{
    public Task<ReadCompanyDto> GetCompany(string authorization, int companyId);
    public Task<ReadCompanyDto> CreateCompany(string authorization, CreateCompanyDto createCompanyDto);
    public Task UpdateCompany(string authorization, UpdateCompanyDto updateCompanyDto);
    public Task UpdateCompanyName(string authorization, UpdateCompanyNameDto updateCompanyNameDto);
    public Task UpdateCompanyEmail(string authorization, UpdateCompanyEmailDto updateCompanyEmailDto);
    public Task UpdateCompanyPostalAddress(string authorization, UpdateCompanyPostalAddressDto updateCompanyPostalAddressDto);
}