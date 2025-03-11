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
    public string UserId { get;  init; }
    public string HotelId { get;  init; }
    public DateTime ReservationDate { get; set; }
}

public class UpdateBookingDto : CreateBookingDto
{
    public Guid Id { get;  init; }
}