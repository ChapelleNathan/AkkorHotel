using Backend.DTO;
using Backend.Models;

namespace Backend.Service.BookingService;

public interface IBookingService
{
    public Task<Booking> CreateBooking(CreateBookingDto booking);
    public Task<Booking> GetBookingById(string id);
    public Task<List<Booking>> GetBookings();
    public Task<Booking> UpdateBooking(UpdateBookingDto booking);
    public Task<Booking> DeleteBooking(string id);
}