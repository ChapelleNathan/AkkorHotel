using Backend.Context;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.BookingRepository;

public class BookingRepository(DataContext context) : IBookingRepository
{
    public void Save()
    {
        context.SaveChanges();
    }

    public async Task<Booking> CreateBooking(Booking booking)
    {
        var newBook = await context.Bookings.AddAsync(booking);
        return newBook.Entity;
    }

    public async Task<Booking?> GetBookingById(string bookingId)
    {
        return await context.Bookings.Where(b => b.Id.ToString().Equals(bookingId))
            .Include(b => b.User)
            .Include(b => b.Hotel)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Booking>> GetBookingsByUserId(string userId)
    {
        return await context.Bookings.Where(b => b.Id.ToString().Equals(userId))
            .Include(b => b.User)
            .Include(b => b.Hotel)
            .ToListAsync();
    }

    public Booking UpdateBooking(Booking booking)
    {
        var updatedBooking = context.Bookings.Update(booking);
        return updatedBooking.Entity;
    }

    public Booking DeleteBooking(Booking booking)
    {
        throw new NotImplementedException();
    }
}