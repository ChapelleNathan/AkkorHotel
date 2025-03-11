using AutoMapper;
using Backend.DTO;
using Backend.HttpResponse;
using Backend.Service.BookingService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller;

[ApiController]
[Route("/api/booking")]
[Authorize]
public class BookingController(IBookingService bookingService, IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<HttpResponse<BookingDto>>> CreateBooking(CreateBookingDto createBookingDto, AppUserDto appUserDto)
    {
        var httpResponse = new HttpResponse<BookingDto>();

        try
        {
            httpResponse.Response = mapper.Map<BookingDto>(await bookingService.CreateBooking(createBookingDto, appUserDto));
        }
        catch (HttpResponseException e)
        {
            httpResponse.HttpCode = e.StatusCode;
            httpResponse.ErrorMessage = e.Message;
        }
        
        return new HttpResponseHandler().Handle(httpResponse);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HttpResponse<BookingDto>>> GetBooking(string id, AppUserDto appUserDto)
    {
        var httpResponse = new HttpResponse<BookingDto>();

        try
        {
            httpResponse.Response = mapper.Map<BookingDto>(await bookingService.GetBookingById(id, appUserDto));
        }
        catch (HttpResponseException e)
        {
            httpResponse.HttpCode = e.StatusCode;
            httpResponse.ErrorMessage = e.Message;
        }
        
        return new HttpResponseHandler().Handle(httpResponse);
    }

    [HttpGet("user/{id}")]
    public async Task<ActionResult<HttpResponse<List<BookingDto>>>> GetBookings(string id, AppUserDto appUserDto)
    {
        var httpResponse = new HttpResponse<List<BookingDto>>();

        try
        {
            var bookings = await bookingService.GetBookingsByUserId(id, appUserDto);
            httpResponse.Response = bookings.Select(mapper.Map<BookingDto>).ToList();
        }
        catch (HttpResponseException e)
        {
            httpResponse.HttpCode = e.StatusCode;
            httpResponse.ErrorMessage = e.Message;
        }
        
        return new HttpResponseHandler().Handle(httpResponse);
    }

    [HttpPut]
    public async Task<ActionResult<HttpResponse<BookingDto>>> UpdateBooking(UpdateBookingDto updateBookingDto, AppUserDto appUserDto)
    {
        var httpResponse = new HttpResponse<BookingDto>();

        try
        {
            httpResponse.Response = mapper.Map<BookingDto>(await bookingService.UpdateBooking(updateBookingDto, appUserDto));
        }
        catch (HttpResponseException e)
        {
            httpResponse.HttpCode = e.StatusCode;
            httpResponse.ErrorMessage = e.Message;
        }
        
        return new HttpResponseHandler().Handle(httpResponse);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<HttpResponse<BookingDto>>> DeleteBooking(string id, AppUserDto appUserDto)
    {
        var httpResponse = new HttpResponse<BookingDto>();

        try
        {
            httpResponse.Response = mapper.Map<BookingDto>(await bookingService.DeleteBooking(id, appUserDto));
        }
        catch (HttpResponseException e)
        {
            httpResponse.HttpCode = e.StatusCode;
            httpResponse.ErrorMessage = e.Message;
        }
        
        return new HttpResponseHandler().Handle(httpResponse);
    }
}