using AutoMapper;
using Backend.Context;
using Backend.Enum;
using Backend.Models;
using Backend.Repository.UserRepository;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;

namespace BackendTest.Context;

public class DataContextTest : IDisposable
{
    private readonly DataContext _context;
    public readonly UserRepository UserRepository;
    public Guid UserId { get; private set; }
    public Guid AdminId { get; private set; }
    
    public DataContextTest()
    {
        var dbName = $"TestDb_{Guid.NewGuid()}";
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        
        _context = new DataContext(options);
        _context.Database.EnsureCreated();
        
        UserRepository = new UserRepository(_context);
        
        SeedDb();
    }

    public void SeedDb()
    {
        var user = new User("test", "user", "test@user.com", BCrypt.Net.BCrypt.HashPassword("Password123."), "defaultUser");
        var admin = new User("admin", "test", "admin@gmail.com", BCrypt.Net.BCrypt.HashPassword("Password123."),
            "admin", RoleEnum.Admin);
        user.Id = Guid.NewGuid();
        admin.Id = Guid.NewGuid();
        UserId = user.Id;
        AdminId = admin.Id;
        _context.Users.Add(user);
        _context.Users.Add(admin);
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
    
    //TODO se renseigner la dessus possiblement utile pour les get by id etc et libérer la db temporaire
}