using AutoMapper;
using SendMail.Models.ContactModels;
using SendMail.Models.Mail;
using SendMail.Models.UserModels;

namespace SendMail.Mappings;

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
