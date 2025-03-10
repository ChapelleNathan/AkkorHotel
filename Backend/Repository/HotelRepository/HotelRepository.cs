using Backend.Context;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.HotelRepository;

public class HotelRepository(DataContext context) : IHotelRepository
{
    public async Task<Hotel> CreateHotel(Hotel hotel)
    {
        var newHotel = await context.Hotels.AddAsync(hotel);
        return newHotel.Entity;
    }

    public async Task<Hotel?> GetHotelById(string id)
    {
        return await context.Hotels.FirstOrDefaultAsync(user => user.Id.ToString().Equals(id));
    }

    public async Task<List<Hotel>> GetHotels()
    {
        return await context.Hotels.ToListAsync();
    }

    public Task<Hotel> UpdateHotel(Hotel hotel)
    {
        throw new NotImplementedException();
    }

    public Hotel DeleteHotel(string id)
    {
        throw new NotImplementedException();
    }

    public void Save()
    {
        context.SaveChanges();
    }
}