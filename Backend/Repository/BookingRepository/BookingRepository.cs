using Backend.Models;

namespace Backend.Repository.BookingRepository;

public class BookingRepository : IBookingRepository
{
    public void Save()
    {
        throw new NotImplementedException();
    }

    public Task<Booking> CreateBooking(Booking booking)
    {
        throw new NotImplementedException();
    }

    public Task<Booking?> GetBookingById(string bookingId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Booking>> GetBookings()
    {
        throw new NotImplementedException();
    }

    public Booking UpdateBooking(Booking booking)
    {
        throw new NotImplementedException();
    }

    public Booking DeleteBooking(Booking booking)
    {
        throw new NotImplementedException();
    }
}