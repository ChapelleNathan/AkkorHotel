using Backend.DTO;
using Backend.Models;

namespace Backend.Service.UserServices;

public interface IUserService
{
    public Task<User> CreateUser(CreateUserDto createdUser);
    public Task<User> GetUserById(string id);
    public Task<User> DeleteUserById(string id);
    public Task<User> UpdateUser(UpdatedUserDto updatedUserDto);
    public Task<List<User>> GetAllUsers();
}