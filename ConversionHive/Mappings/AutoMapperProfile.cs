using AutoMapper;
using ConversionHive.Dtos.ContactDto;
using ConversionHive.Dtos.User;
using ConversionHive.Entities;

namespace ConversionHive.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<CreateContactDto, Contact>().ReverseMap();
        CreateMap<UserRegisterDto, User>().ReverseMap();
        CreateMap<UserDto, User>().ReverseMap();
    }
}
