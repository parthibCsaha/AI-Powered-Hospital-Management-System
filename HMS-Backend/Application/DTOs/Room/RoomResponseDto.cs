
namespace HMS_Backend.Application.DTOs.Room;


public class RoomResponseDto
{
    public Guid Id { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public string RoomType { get; set; } = string.Empty;
    public int Floor { get; set; }
    public bool IsOccupied { get; set; }
    public decimal DailyRate { get; set; }
    public string? Description { get; set; }

}