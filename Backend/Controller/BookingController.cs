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
    public async Task<ActionResult<HttpResponse<BookingDto>>> CreateBooking(CreateBookingDto createBookingDto)
    {
        var httpResponse = new HttpResponse<BookingDto>();

        try
        {
            httpResponse.Response = mapper.Map<BookingDto>(await bookingService.CreateBooking(createBookingDto));
        }
        catch (HttpResponseException e)
        {
            httpResponse.HttpCode = e.StatusCode;
            httpResponse.ErrorMessage = e.Message;
        }
        
        return new HttpResponseHandler().Handle(httpResponse);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HttpResponse<BookingDto>>> GetBooking(string id)
    {
        var httpResponse = new HttpResponse<BookingDto>();

        try
        {
            httpResponse.Response = mapper.Map<BookingDto>(await bookingService.GetBookingById(id));
        }
        catch (HttpResponseException e)
        {
            httpResponse.HttpCode = e.StatusCode;
            httpResponse.ErrorMessage = e.Message;
        }
        
        return new HttpResponseHandler().Handle(httpResponse);
    }

    [HttpGet]
    public async Task<ActionResult<HttpResponse<List<BookingDto>>>> GetBookings()
    {
        var httpResponse = new HttpResponse<List<BookingDto>>();

        try
        {
            var bookings = await bookingService.GetBookings();
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
    public async Task<ActionResult<HttpResponse<BookingDto>>> UpdateBooking(UpdateBookingDto updateBookingDto)
    {
        var httpResponse = new HttpResponse<BookingDto>();

        try
        {
            httpResponse.Response = mapper.Map<BookingDto>(await bookingService.UpdateBooking(updateBookingDto));
        }
        catch (HttpResponseException e)
        {
            httpResponse.HttpCode = e.StatusCode;
            httpResponse.ErrorMessage = e.Message;
        }
        
        return new HttpResponseHandler().Handle(httpResponse);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<HttpResponse<BookingDto>>> DeleteBooking(string id)
    {
        var httpResponse = new HttpResponse<BookingDto>();

        try
        {
            httpResponse.Response = mapper.Map<BookingDto>(await bookingService.DeleteBooking(id));
        }
        catch (HttpResponseException e)
        {
            httpResponse.HttpCode = e.StatusCode;
            httpResponse.ErrorMessage = e.Message;
        }
        
        return new HttpResponseHandler().Handle(httpResponse);
    }
}