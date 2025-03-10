using Backend.DTO;
using Backend.Models;

namespace Backend.Service.HotelService;

public interface IHotelService
{
    public Task<Hotel> CreateHotel(CreateHotelDto hotel);
    public Task<Hotel> GetHotelById(string id);
    public Task<List<Hotel>> GetHotels();
    public Task<Hotel> UpdateHotel(UpdateHotelDto hotel);
    public Task<Hotel> DeleteHotel(string id);
}