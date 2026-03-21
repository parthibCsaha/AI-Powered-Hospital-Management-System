

namespace HMS_Backend.Application.DTOs.Room;



public class CreateRoomDto
{
    public string RoomNumber { get; set; } = string.Empty;
    public string RoomType { get; set; } = string.Empty;
    public int Floor { get; set; }
    public decimal DailyRate { get; set; }
    public string? Description { get; set; }
}