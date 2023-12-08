using AutoMapper;
using ConversionHive.Entities;
using ConversionHive.Models.ContactModels;
using ConversionHive.Models.Mail;
using ConversionHive.Models.UserModels;

namespace ConversionHive.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<MailDto, Mail>().ReverseMap();
        CreateMap<CreateContactDto, Contact>().ReverseMap();
        CreateMap<UserRegisterDto, User>().ReverseMap();
        CreateMap<UserDto, User>().ReverseMap();
        CreateMap<CreateContactResponseDto, Contact>().ReverseMap();
    }
}
