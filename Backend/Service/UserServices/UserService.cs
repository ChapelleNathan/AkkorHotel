using System.Text.RegularExpressions;
using AutoMapper;
using Backend.DTO;
using Backend.Enum;
using Backend.Helper;
using Backend.HttpResponse;
using Backend.Models;
using Backend.Repository.UserRepository;

namespace Backend.Service.UserServices;

public partial class UserService(IUserRepository userRepository, IMapper mapper) : IUserService
{
    public async Task<User> CreateUser(CreateUserDto createdUser)
    {
        var isInDb = await userRepository.GetUserByEmail(createdUser.Email);
        if (isInDb is not null)
            throw new HttpResponseException(401, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup401EmailTaken));
        VerifUser(mapper.Map<User>(createdUser));
        createdUser.Password = BCrypt.Net.BCrypt.HashPassword(createdUser.Password);

        try
        {
            var user = await userRepository.CreateUser(mapper.Map<User>(createdUser));
            userRepository.Save();
            return user;
        }
        catch (Exception e)
        {
            throw new HttpResponseException(500, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup500UnknownError));
        }
    }

    public async Task<User> GetUserById(string id, AppUserDto appUserDto)
    {
        var connectedUser = await userRepository.GetOneUserById(appUserDto.Id);
        VerifyConnectedUser(connectedUser, id);
        
        var user = await userRepository.GetOneUserById(id);
        if (user is null)
            throw new HttpResponseException(404, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup404UserNotFound));

        return user;
    }

    public async Task<User> DeleteUserById(string id, AppUserDto appUserDto)
    {
        var user = await GetUserById(id, appUserDto);
        return userRepository.DeleteUserById(user);
    }

    public async Task<User> UpdateUser(UpdatedUserDto updatedUserDto, AppUserDto appUserDto)
    {
        var user = await GetUserById(updatedUserDto.Id, appUserDto);
        user.Lastname = updatedUserDto.Lastname;
        user.Email = updatedUserDto.Email;
        user.Firstname = updatedUserDto.Firstname;
        user.Pseudo = updatedUserDto.Pseudo;
        VerifUser(user);
        return userRepository.UpdateUser(user);
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await userRepository.GetAllUsers();
    }

    private static void VerifUser(User user)
    {
        if (user.Email.Length > 100)
            throw new HttpResponseException(400, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup400TooLongEmail));
        if (!EmailRegex().Match(user.Email).Success)
            throw new HttpResponseException(400, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup400EmailNotAccepted));
        if(user.Firstname.Length > 100)
            throw new HttpResponseException(400, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup400TooLongFirstname));
        if (user.Lastname.Length > 100)
            throw new HttpResponseException(400, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup400TooLongLastname));
        if (user.Password.Length > 255)
            throw new HttpResponseException(400, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup400TooLongPassword));
        if (user.Password.Length < 8)
            throw new HttpResponseException(400, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup400TooShortPassword));
        if (!PasswordRegex().Match(user.Password).Success)
            throw new HttpResponseException(400, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup400PasswordFormat));
    }

    [GeneratedRegex(@"^[\w\.-]+@([\w\.-]+\.\w{2,}|(\d{1,3}\.){3}\d{1,3})$")]
    private static partial Regex EmailRegex();


    [GeneratedRegex(@"(?=(.*[a-z]{1,}))(?=(.*[A-Z]{1,}))(?=(.*[0-9]{1,}))(?=(.*[!@#$%^&*()\-__+.]{1,})).{8,}")]
    private static partial Regex PasswordRegex();

    private void VerifyConnectedUser(User? connectedUser, string askedUserId)
    {
        if (connectedUser is null)
            throw new HttpResponseException(401, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup401NotConnected));
        
        if(!askedUserId.Equals(connectedUser.Id.ToString()) && !connectedUser.Role.Equals(RoleEnum.Admin))
            throw new HttpResponseException(401, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup401Authorization));
    }
}