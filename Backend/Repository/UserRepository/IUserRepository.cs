using Backend.DTO;
using Backend.Models;

namespace Backend.Repository.UserRepository;

public interface IUserRepository : IRepository
{
    public Task<User> CreateUser(User user);
    public Task<User?> GetOneUserById(string id);
    public User DeleteUserById(User id);
    public User UpdateUser(User user);
    public Task<List<User>> GetAllUsers();
    public Task<User?> GetUserByEmail(string email);
}