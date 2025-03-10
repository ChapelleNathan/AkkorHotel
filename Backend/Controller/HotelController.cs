using AutoMapper;
using Backend.DTO;
using Backend.Filter;
using Backend.HttpResponse;
using Backend.Service.HotelService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller;

[ApiController]
[Route("/api/hotel")]
[Authorize]
[AdminFilter]
public class HotelController(IMapper mapper, IHotelService hotelService)
{
    [HttpPost]
    public async Task<ActionResult<HttpResponse<HotelDto>>> CreateHotelDto (CreateHotelDto createHotelDto)
    {
        var httpResponse = new HttpResponse<HotelDto>();

        try
        {
            httpResponse.Response = mapper.Map<HotelDto>(await hotelService.CreateHotel (createHotelDto));
        }
        catch (HttpResponseException e)
        {
            httpResponse.HttpCode = e.StatusCode;
            httpResponse.ErrorMessage = e.Message;
        }
        
        return new HttpResponseHandler().Handle(httpResponse);
    }

    [HttpGet("{hotelId}")]
    public async Task<ActionResult<HttpResponse<HotelDto>>> GetHotelDto(string hotelId)
    {
        var httpResponse = new HttpResponse<HotelDto>();
        try
        {
            httpResponse.Response = mapper.Map<HotelDto>(await hotelService.GetHotelById(hotelId));
        }
        catch (HttpResponseException e)
        {
            httpResponse.HttpCode = e.StatusCode;
            httpResponse.ErrorMessage = e.Message;
        }
        
        return new HttpResponseHandler().Handle(httpResponse);
    }

    [HttpGet]
    public async Task<ActionResult<HttpResponse<List<HotelDto>>>> GetHotels()
    {
        var httpResponse = new HttpResponse<List<HotelDto>>();

        try
        {
            var hotels = await hotelService.GetHotels();
            httpResponse.Response = hotels.Select(mapper.Map<HotelDto>).ToList();
        }
        catch (HttpResponseException e)
        {
            httpResponse.ErrorMessage = e.Message;
            httpResponse.HttpCode = e.StatusCode;
        }
        
        return new HttpResponseHandler().Handle(httpResponse);
    }

    [HttpPut("update")]
    public async Task<ActionResult<HttpResponse<HotelDto>>> UpdateHotelDto(UpdateHotelDto updateHotelDto)
    {
        var httpResponse = new HttpResponse<HotelDto>();

        try
        {
            httpResponse.Response = mapper.Map<HotelDto>(await hotelService.UpdateHotel(updateHotelDto));
        }
        catch (HttpResponseException e)
        {
            httpResponse.HttpCode = e.StatusCode;
            httpResponse.ErrorMessage = e.Message;
        }
        
        return new HttpResponseHandler().Handle(httpResponse);
    }

    [HttpDelete("{hotelId}")]
    public async Task<ActionResult<HttpResponse<HotelDto>>> DeleteHotel(string hotelId)
    {
        var httpResponse = new HttpResponse<HotelDto>();

        try
        {
            httpResponse.Response = mapper.Map<HotelDto>(await hotelService.DeleteHotel(hotelId));
        }
        catch (HttpResponseException e)
        {
            httpResponse.HttpCode = e.StatusCode;
            httpResponse.ErrorMessage = e.Message;
        }
        
        return new HttpResponseHandler().Handle(httpResponse);
    }
}