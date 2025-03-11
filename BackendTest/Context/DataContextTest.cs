using AutoMapper;
using Backend.Context;
using Backend.Enum;
using Backend.Models;
using Backend.Repository.HotelRepository;
using Backend.Repository.UserRepository;
using Bogus;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;

namespace BackendTest.Context;

public class DataContextTest : IDisposable
{
    private readonly DataContext _context;
    public readonly UserRepository UserRepository;
    public readonly HotelRepository HotelRepository;
    private readonly Faker _faker = new Faker();
    public Guid UserId { get; private set; }
    public Guid AdminId { get; private set; }
    public Guid HotelId { get; private set; }
    
    public DataContextTest()
    {
        var dbName = $"TestDb_{Guid.NewGuid()}";
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        
        _context = new DataContext(options);
        _context.Database.EnsureCreated();
        
        UserRepository = new UserRepository(_context);
        HotelRepository = new HotelRepository(_context);
        
        SeedDb();
    }

    public void SeedDb()
    {
        SeedUsers();
        SeedHotels();
        _context.SaveChanges();
    }
    
    private void SeedUsers()
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
    }

    private void SeedHotels()
    {
        var hotel = new Hotel("Companie créole", "Bahamas","Au bal masqué ohé ohé");
        hotel.Id = Guid.NewGuid();
        HotelId = hotel.Id;
        _context.Hotels.Add(hotel);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
    
    //TODO se renseigner la dessus possiblement utile pour les get by id etc et libérer la db temporaire
}