using AutoMapper;
using Backend.DTO;
using Backend.Models;

namespace Backend;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        //User
        CreateMap<UserDto, User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore());
        CreateMap<CreateUserDto, User>();
        CreateMap<User, UserDto>();
        CreateMap<UpdatedUserDto, User>();
    }
}