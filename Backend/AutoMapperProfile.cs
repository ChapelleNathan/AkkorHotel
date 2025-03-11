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
        
        //Hotel
        CreateMap<HotelDto, Hotel>();
        CreateMap<Hotel, HotelDto>();
        CreateMap<CreateHotelDto, Hotel>();
        CreateMap<UpdateHotelDto, Hotel>();
        
        //Booking
        CreateMap<BookingDto, Booking>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.UserDto))
            .ForMember(dest => dest.Hotel, opt => opt.MapFrom(src => src.HotelDto))
            .ReverseMap();
    }
}