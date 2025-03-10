namespace Backend.DTO;

public class HotelDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public string Description { get; set; }
}

public class CreateHotelDto
{
    public string Name { get; set; }
    public string Location { get; set; }
    public string Description { get; set; } = "";
}

public class UpdateHotelDto : CreateHotelDto
{
    public Guid Id { get; set; }
}