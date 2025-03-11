using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public class Booking
{
    
    public Booking(){}

    public Booking(User user, Hotel hotel, DateTime requestedDate)
    {
        User = user;
        Hotel = hotel;
        ReservationDate = requestedDate;
    }
    
    [Key]
    public Guid Id { get; init; }

    [Column("user_id")]
    [Required]
    public User User { get; private set; }

    [Column("hotel_id")]
    [Required]
    public Hotel Hotel { get; private set; }
    
    [Column("reservation_date")]
    public DateTime ReservationDate { get; set; }
}