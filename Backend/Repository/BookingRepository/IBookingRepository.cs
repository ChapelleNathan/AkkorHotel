using Backend.Models;

namespace Backend.Repository.BookingRepository;

public interface IBookingRepository : IRepository
{
    public Task<Booking> CreateBooking(Booking booking);
    public Task<Booking?> GetBookingById(string bookingId);
    public Task<List<Booking>> GetBookings();
    public Booking UpdateBooking(Booking booking);
    public Booking DeleteBooking(Booking booking);
}