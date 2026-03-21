


namespace HMS_Backend.Application.DTOs.Admission;


public class CreateAdmissionDto
{
    public Guid PatientId { get; set; }
    public Guid DoctorId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime AdmissionDate { get; set; }
    public string AdmissionReason { get; set; } = string.Empty;

}