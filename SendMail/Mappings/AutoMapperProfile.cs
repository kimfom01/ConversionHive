using AutoMapper;
using SendMail.Models;

namespace SendMail.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<MailDto, Mail>().ReverseMap();
        CreateMap<ContactDto, Contact>().ReverseMap();
    }
}
