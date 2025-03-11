using System.Security.Claims;
using AutoMapper;
using Backend.DTO;
using Backend.Enum;
using Backend.Helper;
using Backend.HttpResponse;
using Backend.Models;
using Backend.Repository.BookingRepository;
using Backend.Service.BookingService;
using Backend.Service.HotelService;
using Backend.Service.UserServices;
using BackendTest.Context;
using Bogus;
using FakeItEasy;
using Microsoft.AspNetCore.Http;

namespace BackendTest.Service;

public class BookingServiceTest : IClassFixture<DataContextTest>
{
    private readonly DataContextTest _context;
    private readonly BookingService _bookingService, _fakeBookingService;
    private readonly HttpContext _fakeHttpContext = A.Fake<HttpContext>();
    private readonly IHttpContextAccessor _fakeHttpContextAccessor = A.Fake<IHttpContextAccessor>();
    private readonly IUserService _fakeUserService = A.Fake<IUserService>();
    private readonly IHotelService _fakeHotelService = A.Fake<IHotelService>();
    private readonly Faker _faker = new Faker();
    private readonly IBookingRepository _fakeBookingRepository = A.Fake<IBookingRepository>();
    private readonly AppUserDto _appUserDto, _appAdminDto;

    public BookingServiceTest(DataContextTest context)
    {
        _context = context;
        _appUserDto = CreateFakeAppUserDto(_context.User.Email, _context.User.Id.ToString(), RoleEnum.User);
        _appAdminDto = CreateFakeAppUserDto(_context.Admin.Email, _context.Admin.Id.ToString(), RoleEnum.Admin);
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserDto, User>().ReverseMap();
            cfg.CreateMap<HotelDto, Hotel>().ReverseMap();
            cfg.CreateMap<BookingDto, Booking>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.UserDto))
                .ForMember(dest => dest.Hotel, opt => opt.MapFrom(src => src.HotelDto))
                .ReverseMap();
        });
        var mapper = mapperConfig.CreateMapper();
        _bookingService = new BookingService(mapper, context.BookingRepository, _fakeUserService, _fakeHotelService);
        _fakeBookingService = new BookingService(mapper, _fakeBookingRepository, _fakeUserService, _fakeHotelService);
    }

    //Create
    [Fact]
    public async Task CreateBooking_Test()
    {
        var createBookingDto = CreateFakeBookingDto(_context.User.Id.ToString(), _context.Hotel.Id.ToString());
        
        A.CallTo(() => _fakeUserService.GetUserById(createBookingDto.UserId, _appUserDto)).Returns(Task.FromResult(_context.User));
        A.CallTo(() => _fakeHotelService.GetHotelById(createBookingDto.HotelId)).Returns(Task.FromResult(_context.Hotel));
        
        var booking = await _bookingService.CreateBooking(createBookingDto, _appUserDto);
        
        Assert.NotNull(booking);
        Assert.Equal(createBookingDto.ReservationDate, booking.ReservationDate);
        Assert.Equal(createBookingDto.UserId, booking.User.Id.ToString());
        Assert.Equal(createBookingDto.HotelId, booking.Hotel.Id.ToString());
    }

    [Fact]
    public async Task CreateUser_UnexpectedError_Test()
    {
        var createBookingDto = CreateFakeBookingDto(_context.User.Id.ToString(), _context.Hotel.Id.ToString());
        A.CallTo(() => _fakeUserService.GetUserById(createBookingDto.UserId, _appUserDto)).Returns(Task.FromResult(_context.User));
        A.CallTo(() => _fakeHotelService.GetHotelById(createBookingDto.HotelId)).Returns(Task.FromResult(_context.Hotel));
        A.CallTo(() => _fakeBookingRepository.CreateBooking(A<Booking>._)).Throws<Exception>();
        
        var exception = await Assert.ThrowsAsync<HttpResponseException>(() => _fakeBookingService.CreateBooking(createBookingDto, _appUserDto));
        
        Assert.True(exception.StatusCode.Equals(500));
        Assert.Equal(ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup500UnknownError), exception.Message);
    }

    private CreateBookingDto CreateFakeBookingDto(string userId, string hotelId)
    {
        return new CreateBookingDto
        {
            UserId = userId,
            HotelId = hotelId,
            ReservationDate = _faker.Date.Future().ToUniversalTime()
        };
    }
    
    private AppUserDto CreateFakeAppUserDto(string email, string id, RoleEnum role)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.NameIdentifier, id),
            new Claim(ClaimTypes.Role, role.ToString()),
        };
        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);
        A.CallTo(() => _fakeHttpContext.User).Returns(claimsPrincipal);
        A.CallTo(() => _fakeHttpContextAccessor.HttpContext).Returns(_fakeHttpContext);
        return new AppUserDto(_fakeHttpContextAccessor);
    }
}