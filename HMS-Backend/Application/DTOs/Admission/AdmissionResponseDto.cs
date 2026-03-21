

namespace HMS_Backend.Application.DTOs.Admission;


 
public class AdmissionResponseDto
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public Guid DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public Guid RoomId { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public string RoomType { get; set; } = string.Empty;
    public DateTime AdmissionDate { get; set; }
    public DateTime? DischargeDate { get; set; }
    public string AdmissionReason { get; set; } = string.Empty;
    public string? DischargeNotes { get; set; }
    public decimal? TotalBill { get; set; }
    public bool IsDischarged { get; set; }
    public int? DaysAdmitted { get; set; }

}