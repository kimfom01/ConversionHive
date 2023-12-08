using AutoMapper;
using ConversionHive.Dtos.ContactDto;
using ConversionHive.Dtos.User;
using ConversionHive.Entities;
using ConversionHive.Models.Mail;

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
