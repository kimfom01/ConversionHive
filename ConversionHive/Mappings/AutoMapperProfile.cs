using AutoMapper;
using ConversionHive.Dtos.CompanyDto;
using ConversionHive.Dtos.ContactDto;
using ConversionHive.Dtos.MailConfigDto;
using ConversionHive.Dtos.UserDto;
using ConversionHive.Entities;

namespace ConversionHive.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Company, CreateCompanyDto>().ReverseMap();
        CreateMap<Company, ReadCompanyDto>().ReverseMap();
        CreateMap<Company, UpdateCompanyDto>().ReverseMap();
        CreateMap<Company, UpdateCompanyNameDto>().ReverseMap();
        CreateMap<Company, UpdateCompanyEmailDto>().ReverseMap();
        CreateMap<Company, UpdateCompanyPostalAddressDto>().ReverseMap();
        CreateMap<Contact, CreateContactDto>().ReverseMap();
        CreateMap<Contact, CreateContactCsvDto>().ReverseMap();
        CreateMap<Contact, ReadContactDto>().ReverseMap();
        CreateMap<Contact, UpdateContactDto>().ReverseMap();
        CreateMap<MailConfig, CreateMailConfigDto>().ReverseMap();
        CreateMap<MailConfig, ReadMailConfigDto>().ReverseMap();
        CreateMap<MailConfig, UpdateMailConfigDto>().ReverseMap();
        CreateMap<User, ReadUserDto>().ReverseMap();
        CreateMap<User, UserLoginDto>().ReverseMap();
        CreateMap<User, UserRegisterDto>().ReverseMap();
    }
}