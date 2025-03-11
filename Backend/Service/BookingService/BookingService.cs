using Backend.DTO;
using Backend.Models;

namespace Backend.Service.BookingService;

public class BookingService : IBookingService
{
    public Task<Booking> CreateBooking(CreateBookingDto booking)
    {
        throw new NotImplementedException();
    }

    public Task<Booking> GetBookingById(string id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Booking>> GetBookings()
    {
        throw new NotImplementedException();
    }

    public Task<Booking> UpdateBooking(UpdateBookingDto booking)
    {
        throw new NotImplementedException();
    }

    public Task<Booking> DeleteBooking(string id)
    {
        throw new NotImplementedException();
    }
}