using AutoMapper;
using Backend.DTO;
using Backend.HttpResponse;
using Backend.Service.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller;

[ApiController]
[Route("/api/user")]
public class UserController(IUserService userService, IMapper mapper): ControllerBase
{
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<HttpResponse<UserDto>>> GetUser(string id, AppUserDto user)
    {
        var httpResponse = new HttpResponse<UserDto>();
        try
        {
            httpResponse.Response = mapper.Map<UserDto>(await userService.GetUserById(id, user));
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
    public async Task<ActionResult<HttpResponse<UserDto>>> DeleteUser(string id, AppUserDto user)
    {
        var httpResponse = new HttpResponse<UserDto>();
        try
        {
            httpResponse.Response = mapper.Map<UserDto>(await userService.DeleteUserById(id, user));
        }
        catch (HttpResponseException e)
        {
            httpResponse.HttpCode = e.StatusCode;
            httpResponse.ErrorMessage = e.Message;
        }

        return new HttpResponseHandler().Handle(httpResponse);
    }

    [HttpPut("update")]
    public async Task<ActionResult<HttpResponse<UserDto>>> UpdateUser([FromBody]UpdatedUserDto updatedUserDto, AppUserDto user)
    {
        var httpResponse = new HttpResponse<UserDto>();
        try
        {
            httpResponse.Response = mapper.Map<UserDto>(await userService.UpdateUser(updatedUserDto, user));
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