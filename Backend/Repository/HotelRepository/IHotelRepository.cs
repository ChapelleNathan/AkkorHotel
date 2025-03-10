using Backend.Models;

namespace Backend.Repository.HotelRepository;

public interface IHotelRepository
{
    public Hotel CreateHotel(Hotel hotel);
    public Hotel GetHotelById(string id);
    public IEnumerable<Hotel> GetHotels();
    public Hotel UpdateHotel(Hotel hotel);
    public Hotel DeleteHotel(string id);
}