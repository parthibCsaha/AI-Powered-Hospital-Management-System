
namespace HMS_Backend.Application.DTOs.Department;


public class DepartmentResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string PhoneExtension { get; set; } = string.Empty;
    public int DoctorCount { get; set; }
    public DateTime CreatedAt { get; set; }

}