

using HMS_Backend.Application.Interfaces;


namespace HMS_Backend.Application.Services;



public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _uow;
 
    public DashboardService(IUnitOfWork uow) { _uow = uow; }
 
    public async Task<DashboardStatsDto> GetStatsAsync(CancellationToken ct = default)
    {
        var today = DateTime.UtcNow.Date;
        return new DashboardStatsDto
        {
            TotalPatients = await _uow.Patients.CountAsync(p => !p.IsDeleted, ct),

            TotalDoctors = await _uow.Doctors.CountAsync(d => !d.IsDeleted, ct),

            TotalDepartments = await _uow.Departments.CountAsync(d => !d.IsDeleted, ct),

            TodayAppointments = await _uow.Appointments.CountAsync(
                a => a.AppointmentDate.Date == today && !a.IsDeleted, ct),

            PendingAppointments = await _uow.Appointments.CountAsync(
                a => (int)a.Status <= 2 && !a.IsDeleted, ct),

            CurrentAdmissions = await _uow.Admissions.CountAsync(
                a => !a.IsDischarged && !a.IsDeleted, ct),

            AvailableRooms = await _uow.Rooms.CountAsync(r => !r.IsOccupied && !r.IsDeleted, ct),

            OccupiedRooms = await _uow.Rooms.CountAsync(r => r.IsOccupied && !r.IsDeleted, ct)
        };
    }
}
