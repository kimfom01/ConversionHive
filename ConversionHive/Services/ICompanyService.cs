using ConversionHive.Dtos.Company;

namespace ConversionHive.Services;

public interface ICompanyService
{
    public Task<ReadCompanyDto> GetCompany(string authorization, int companyId);
    public Task<ReadCompanyDto> CreateCompany(string authorization, CreateCompanyDto createCompanyDto);
    public Task UpdateCompany(string authorization, UpdateCompanyDto updateCompanyDto);
}