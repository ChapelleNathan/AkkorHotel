using AutoMapper;
using Backend.DTO;
using Backend.Enum;
using Backend.Helper;
using Backend.HttpResponse;
using Backend.Models;
using Backend.Repository.HotelRepository;

namespace Backend.Service.HotelService;

public class HotelService(IHotelRepository hotelRepository, IMapper mapper) : IHotelService
{
    public async  Task<Hotel> CreateHotel(CreateHotelDto createdHotelDto)
    {
        var hotel = mapper.Map<Hotel>(createdHotelDto);
        VerifyHotelInput(hotel);

        try
        {
            var newHotel = await hotelRepository.CreateHotel(hotel);
            hotelRepository.Save();
            return newHotel;
        }
        catch (Exception e)
        {
            throw new HttpResponseException(500, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup500UnknownError));
        }
    }

    public async Task<Hotel> GetHotelById(string id)
    {
        var hotel = await hotelRepository.GetHotelById(id);
        if (hotel is null)
            throw new HttpResponseException(404, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup404HotelNotFound));
        return hotel;
    }

    public async Task<List<Hotel>> GetHotels()
    {
        return await hotelRepository.GetHotels();
    }

    public Task<Hotel> UpdateHotel(UpdateHotelDto hotel)
    {
        throw new NotImplementedException();
    }

    public Task<Hotel> DeleteHotel(string id)
    {
        throw new NotImplementedException();
    }

    private void VerifyHotelInput(Hotel hotel)
    {
        if (hotel.Description.Length > 255)
            throw new HttpResponseException(400,
                ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup400TooLongHotelDescription));
        if (hotel.Name.Length > 50)
            throw new HttpResponseException(400, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup400TooLongHotelName));
        if (hotel.Location.Length > 50)
            throw new HttpResponseException(400, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup400TooLongHotelLocation));
    }
}