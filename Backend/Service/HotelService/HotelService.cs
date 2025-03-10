using Backend.DTO;
using Backend.Models;

namespace Backend.Service.HotelService;

public class HotelService : IHotelService
{
    public Task<Hotel> CreateHotel(CreateHotelDto hotel)
    {
        throw new NotImplementedException();
    }

    public Task<Hotel> GetHotelById(string id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Hotel>> GetHotels()
    {
        throw new NotImplementedException();
    }

    public Task<Hotel> UpdateHotel(UpdateHotelDto hotel)
    {
        throw new NotImplementedException();
    }

    public Task<Hotel> DeleteHotel(string id)
    {
        throw new NotImplementedException();
    }
}