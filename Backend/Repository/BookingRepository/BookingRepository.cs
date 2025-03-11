using Backend.Context;
using Backend.Models;

namespace Backend.Repository.BookingRepository;

public class BookingRepository(DataContext context) : IBookingRepository
{
    public void Save()
    {
        context.SaveChanges();
    }

    public async Task<Booking> CreateBooking(Booking booking)
    {
        var newBook = await context.AddAsync(booking);
        return newBook.Entity;
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