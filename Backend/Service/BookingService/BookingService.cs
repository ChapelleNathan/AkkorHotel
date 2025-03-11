using AutoMapper;
using Backend.DTO;
using Backend.Enum;
using Backend.Helper;
using Backend.Models;
using Backend.Repository.BookingRepository;
using Backend.HttpResponse;
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
        if (bookingDto.ReservationDate < DateTime.Now)
            throw new HttpResponseException(401, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup401BookinPast));
        
        var user = await userService.GetUserById(bookingDto.UserId, appUser);
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

    public async Task<Booking> GetBookingById(string id, AppUserDto appUser)
    {
        var booking = await bookingRepository.GetBookingById(id);
        if (booking is null)
            throw new HttpResponseException(404, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup404BookingNotFound));
        VerifyUser(booking.User.Id.ToString(), appUser);
        

        return booking;
    }

    public async Task<List<Booking>> GetBookingsByUserId(string id, AppUserDto appUser)
    {
        VerifyUser(id, appUser);

        var bookings = await bookingRepository.GetBookingsByUserId(id);
        return bookings;
    }

    public async Task<Booking> UpdateBooking(UpdateBookingDto bookingDto, AppUserDto appUser)
    {
        VerifyUser(bookingDto.UserId, appUser);
        var booking = await bookingRepository.GetBookingById(bookingDto.Id.ToString());
        if(booking is null)
            throw new HttpResponseException(404, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup404BookingNotFound));
        
        booking.ReservationDate = bookingDto.ReservationDate.ToUniversalTime();

        try
        {
            booking = bookingRepository.UpdateBooking(booking);
            bookingRepository.Save();
            return booking;
        }
        catch (Exception e)
        {
            throw new HttpResponseException(500, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup500UnknownError));
        }
    }

    public async Task<Booking> DeleteBooking(string id, AppUserDto appUser)
    {
        var booking = await GetBookingById(id, appUser);
        try
        {
            booking = bookingRepository.DeleteBooking(booking);
            bookingRepository.Save();
            return booking;
        }
        catch (Exception e)
        {
            throw new HttpResponseException(500, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup500UnknownError));
        }
    }

    private void VerifyUser(string id, AppUserDto appUser)
    {
        if (!appUser.Id.Equals(id) && !appUser.Role.Equals(RoleEnum.Admin.ToString()))
            throw new HttpResponseException(401, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup401Authorization));
    }
}