using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Context;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options): base(options) {}
    public DataContext() {}

    public DbSet<User> Users => Set<User>();
    public DbSet<Hotel> Hotels => Set<Hotel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
    }
}