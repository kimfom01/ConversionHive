using AutoMapper;
using SendMail.Models;

namespace SendMail.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<SendMailDto, Mail>().ReverseMap();
    }
}
