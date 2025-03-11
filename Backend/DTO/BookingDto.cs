namespace Backend.DTO;

public class BookingDto
{
    public required Guid Id { get; set; }
    public required UserDto UserDto { get; set; }
    public required HotelDto HotelDto { get; set; }
    public required DateTime ReservationDate { get; set; }
}

public class CreateBookingDto
{
    public string UserId { get; set; }
    public string HotelId { get; set; }
    public DateTime ReservationDate { get; set; }
}

public class UpdateBookingDto
{
    public required DateTime ReservationDate { get; set; }
}