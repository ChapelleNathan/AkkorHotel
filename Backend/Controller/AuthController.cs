using AutoMapper;
using Backend.DTO;
using Backend.HttpResponse;
using Backend.Service.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller;

[ApiController]
[Route("auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("/login")]
    public async Task<ActionResult<HttpResponse<string>>> Login(ConnectUserDto userDto)
    {
        var httpResponse = new HttpResponse<string>();

        try
        {
            httpResponse.Response = await authService.Login(userDto);
        }
        catch (HttpResponseException e)
        {
            httpResponse.HttpCode = e.StatusCode;
            httpResponse.ErrorMessage = e.Message;
        }
        
        return new HttpResponseHandler().Handle(httpResponse);
    }
}