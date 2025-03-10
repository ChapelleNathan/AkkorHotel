using Backend.Context;
using Backend.Models;

namespace Backend.Repository.HotelRepository;

public class HotelRepository(DataContext context) : IHotelRepository
{
    public async Task<Hotel> CreateHotel(Hotel hotel)
    {
        var newHotel = await context.Hotels.AddAsync(hotel);
        return newHotel.Entity;
    }

    public Task<Hotel> GetHotelById(string id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Hotel>> GetHotels()
    {
        throw new NotImplementedException();
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