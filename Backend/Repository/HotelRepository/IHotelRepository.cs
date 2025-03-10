using Backend.Models;

namespace Backend.Repository.HotelRepository;

public interface IHotelRepository : IRepository
{
    public Task<Hotel> CreateHotel(Hotel hotel);
    public Task<Hotel?> GetHotelById(string id);
    public Task<List<Hotel>> GetHotels();
    public Hotel UpdateHotel(Hotel hotel);
    public Hotel DeleteHotel(Hotel hotel);
}