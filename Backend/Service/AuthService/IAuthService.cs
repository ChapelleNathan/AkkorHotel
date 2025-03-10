using Backend.DTO;
using Backend.Models;

namespace Backend.Service.AuthService;

public interface IAuthService
{
    public Task<string> Login(ConnectUserDto userDto);
}