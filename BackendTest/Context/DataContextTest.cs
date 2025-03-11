using AutoMapper;
using Backend.Context;
using Backend.DTO;
using Backend.Enum;
using Backend.Models;
using Backend.Repository.BookingRepository;
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
    public readonly BookingRepository BookingRepository;
    private readonly Faker _faker = new Faker();
    public User User { get; private set; }
    public User Admin { get; private set; }
    public Hotel Hotel { get; private set; }
    public Booking Booking { get; private set; }
    
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
        BookingRepository = new BookingRepository(_context);
        
        SeedDb();
    }

    public void SeedDb()
    {
        SeedUsers();
        SeedHotels();
        SeedBookings();
        _context.SaveChanges();
    }
    
    private void SeedUsers()
    {
        var user = new User("test", "user", "test@user.com", BCrypt.Net.BCrypt.HashPassword("Password123."), "defaultUser");
        var admin = new User("admin", "test", "admin@gmail.com", BCrypt.Net.BCrypt.HashPassword("Password123."),
            "admin", RoleEnum.Admin);
        User = user;
        Admin = admin;
        _context.Users.Add(user);
        _context.Users.Add(admin);
    }

    private void SeedHotels()
    {
        var hotel = new Hotel("Companie créole", "Bahamas","Au bal masqué ohé ohé");
        Hotel = hotel;
        _context.Hotels.Add(hotel);
    }

    private void SeedBookings()
    {
        var booking = new Booking(Admin, Hotel, DateTime.Now.ToUniversalTime());
        Booking = booking;
        _context.Bookings.Add(booking);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
    
    //TODO se renseigner la dessus possiblement utile pour les get by id etc et libérer la db temporaire
}