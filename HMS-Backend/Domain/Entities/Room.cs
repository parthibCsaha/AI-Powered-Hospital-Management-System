using HMS_Backend.Domain.Common;
using HMS_Backend.Domain.Enums;

namespace HMS_Backend.Domain.Entities;

public class Room : BaseEntity
{
    public string RoomNumber { get; set; } = string.Empty;
    public RoomType RoomType { get; set; } 
    public int Floor { get; set; }
    public bool IsOccupied { get; set; } = false;
    public decimal DailyRate { get; set; }
    public string? Description { get; set; }

    // Navigation
    public ICollection<Admission> Admissions { get; set; } = new List<Admission>();
    
}