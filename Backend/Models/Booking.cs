using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public class Booking
{
    
    public Booking(){}

    public Booking(User user, Hotel hotel)
    {
        User = user;
        Hotel = hotel;
    }
    
    [Key]
    public Guid Id { get; set; }

    [Column("user_id")]
    [Required]
    public required User User { get; set; }

    [Column("hotel_id")]
    [Required]
    public required Hotel Hotel { get; set; }
    
    [Column("reservation_date")]
    public required DateTime ReservationDate { get; set; } = DateTime.Now;
}