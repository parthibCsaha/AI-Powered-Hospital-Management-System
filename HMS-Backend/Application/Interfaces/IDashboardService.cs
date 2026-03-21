

namespace HMS_Backend.Application.Interfaces;


public interface IDashboardService
{
    Task<DashboardStatsDto> GetStatsAsync(CancellationToken ct = default);
}
 
public class DashboardStatsDto
{
    public int TotalPatients { get; set; }
    public int TotalDoctors { get; set; }
    public int TotalDepartments { get; set; }
    public int TodayAppointments { get; set; }
    public int PendingAppointments { get; set; }
    public int CurrentAdmissions { get; set; }
    public int AvailableRooms { get; set; }
    public int OccupiedRooms { get; set; }
}