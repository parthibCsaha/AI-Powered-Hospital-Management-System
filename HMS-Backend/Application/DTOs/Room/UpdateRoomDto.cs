


namespace HMS_Backend.Application.DTOs.Room;


public class UpdateRoomDto
{
    public string RoomType { get; set; } = string.Empty;
    public decimal DailyRate { get; set; }
    public string? Description { get; set; }
}