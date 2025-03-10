using AutoMapper;
using Backend.DTO;
using Backend.HttpResponse;
using Backend.Service.UserServices;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller;

[ApiController]
[Route("user")]
public class UserController(IUserService userService, IMapper mapper): ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<HttpResponse<UserDto>>> GetUser(string id)
    {
        var httpResponse = new HttpResponse<UserDto>();
        try
        {
            httpResponse.Response = mapper.Map<UserDto>(await userService.GetUserById(id));
        }
        catch (HttpResponseException e)
        {
            httpResponse.ErrorMessage = e.Message;
            httpResponse.HttpCode = e.StatusCode;
        }
        
        return new HttpResponseHandler().Handle(httpResponse);
    }

    [HttpPost]
    public async Task<ActionResult<HttpResponse<UserDto>>> CreateUser(CreateUserDto createUserDto)
    {
        var httpResponse = new HttpResponse<UserDto>();
        try
        {
            httpResponse.Response = mapper.Map<UserDto>(await userService.CreateUser(createUserDto));
        }
        catch (HttpResponseException e)
        {
            httpResponse.ErrorMessage = e.Message;
            httpResponse.HttpCode = e.StatusCode;
        }

        return new HttpResponseHandler().Handle(httpResponse);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<HttpResponse<UserDto>>> DeleteUser(string id)
    {
        var httpResponse = new HttpResponse<UserDto>();
        try
        {
            httpResponse.Response = mapper.Map<UserDto>(await userService.DeleteUserById(id));
        }
        catch (HttpResponseException e)
        {
            httpResponse.HttpCode = e.StatusCode;
            httpResponse.ErrorMessage = e.Message;
        }

        return new HttpResponseHandler().Handle(httpResponse);
    }

    [HttpPut("/update")]
    public async Task<ActionResult<HttpResponse<UserDto>>> UpdateUser(UpdatedUserDto updatedUserDto)
    {
        var httpResponse = new HttpResponse<UserDto>();
        try
        {
            httpResponse.Response = mapper.Map<UserDto>(await userService.UpdateUser(updatedUserDto));
        }
        catch (HttpResponseException e)
        {
            httpResponse.HttpCode = e.StatusCode;
            httpResponse.ErrorMessage = e.Message;
        }

        return new HttpResponseHandler().Handle(httpResponse);
    }

    [HttpGet]
    public async Task<ActionResult<HttpResponse<List<UserDto>>>> GetAllUsers()
    {
        var httpResponse = new HttpResponse<List<UserDto>>();
        try
        {
            var users = await userService.GetAllUsers();
            httpResponse.Response = users.Select(mapper.Map<UserDto>).ToList();
        }
        catch (Exception e)
        {
            httpResponse.ErrorMessage = e.Message;
            httpResponse.HttpCode = 500;
        }

        return new HttpResponseHandler().Handle(httpResponse);
    }
}