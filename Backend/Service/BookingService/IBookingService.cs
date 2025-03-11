using Backend.DTO;
using Backend.Models;

namespace Backend.Service.BookingService;

public interface IBookingService
{
    public Task<Booking> CreateBooking(CreateBookingDto bookingDto, AppUserDto appUser);
    public Task<Booking> GetBookingById(string id, AppUserDto appUser);
    public Task<List<Booking>> GetBookingsByUserId(string id, AppUserDto appUser);
    public Task<Booking> UpdateBooking(UpdateBookingDto bookingDto, AppUserDto appUser);
    public Task<Booking> DeleteBooking(string id, AppUserDto appUser);
}