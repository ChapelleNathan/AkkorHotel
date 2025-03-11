using AutoMapper;
using Backend.DTO;
using Backend.Enum;
using Backend.Helper;
using Backend.Models;
using Backend.Repository.BookingRepository;
using Backend.HttpResponse;
using Backend.Repository.HotelRepository;
using Backend.Repository.UserRepository;
using Backend.Service.HotelService;
using Backend.Service.UserServices;

namespace Backend.Service.BookingService;

public class BookingService(
    IMapper mapper,
    IBookingRepository bookingRepository,
    IUserService userService,
    IHotelService hotelService) : IBookingService
{
    public async Task<Booking> CreateBooking(CreateBookingDto bookingDto, AppUserDto appUser)
    {
        var user = await userService.GetUserById(appUser.Id, appUser);
        var hotel = await hotelService.GetHotelById(bookingDto.HotelId);
        var booking = new Booking(user, hotel, bookingDto.ReservationDate);
        
        try
        {
            booking = await bookingRepository.CreateBooking(booking);
            bookingRepository.Save();
            return booking;
        }
        catch (Exception e)
        {
            throw new HttpResponseException(500, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup500UnknownError));
        }
    }

    public Task<Booking> GetBookingById(string id, AppUserDto appUser)
    {
        throw new NotImplementedException();
    }

    public Task<List<Booking>> GetBookings(AppUserDto appUser)
    {
        throw new NotImplementedException();
    }

    public Task<Booking> UpdateBooking(UpdateBookingDto bookingDto, AppUserDto appUser)
    {
        throw new NotImplementedException();
    }

    public Task<Booking> DeleteBooking(string id, AppUserDto appUser)
    {
        throw new NotImplementedException();
    }
}