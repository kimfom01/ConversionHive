using AutoMapper;
using SendMail.Models;
using SendMail.Models.Contact;
using SendMail.Models.User;

namespace SendMail.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<MailDto, Mail>().ReverseMap();
        CreateMap<ContactDto, Contact>().ReverseMap();
        CreateMap<UserRegisterDto, User>().ReverseMap();
        CreateMap<UserDto, User>().ReverseMap();
    }
}
