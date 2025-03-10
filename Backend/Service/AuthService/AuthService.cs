using Backend.DTO;
using Backend.Enum;
using Backend.Helper;
using Backend.HttpResponse;
using Backend.Models;
using Backend.Repository.UserRepository;

namespace Backend.Service.AuthService;

public class AuthService(IUserRepository userRepository, AuthHelper authHelper) : IAuthService
{
    public async Task<string> Login(ConnectUserDto userDto)
    {
        var user = await userRepository.GetUserByEmail(userDto.Email);
        if (user is null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password))
            throw new HttpResponseException(401, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup401WrongCredential));

        return authHelper.GenerateToken(user);
    }
}