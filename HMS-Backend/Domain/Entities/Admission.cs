using HMS_Backend.Domain.Common;
using HMS_Backend.Domain.Enums;

namespace HMS_Backend.Domain.Entities;


public class Admission : BaseEntity
{
    public Guid PatientId { get; set; }
    public Guid DoctorId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime AdmissionDate { get; set; }
    public DateTime? DischargeDate { get; set; }
    public string AdmissionReason { get; set; } = string.Empty;
    public string? DischargeNotes { get; set; }
    public decimal? TotalBill { get; set; }
    public bool IsDischarged { get; set; } = false;
 
    // Navigation
    public Patient Patient { get; set; } = null!;
    public Doctor Doctor { get; set; } = null!;
    public Room Room { get; set; } = null!;

}