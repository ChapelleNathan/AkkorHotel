using AutoMapper;
using Backend.Context;
using Backend.Models;
using Backend.Repository.UserRepository;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;

namespace BackendTest.Context;

public class DataContextTest : IDisposable
{
    private readonly DataContext _context;
    public readonly UserRepository UserRepository;
    public Guid UserId { get; set; }
    
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
        var user = new User("test", "user", "test@user.com", BCrypt.Net.BCrypt.HashPassword("Password123."));
        user.Id = Guid.NewGuid();
        UserId = user.Id;
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
    
    //TODO se renseigner la dessus possiblement utile pour les get by id etc et libérer la db temporaire
}