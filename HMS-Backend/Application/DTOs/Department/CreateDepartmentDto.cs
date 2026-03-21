

namespace HMS_Backend.Application.DTOs.Department;

public class CreateDepartmentDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string PhoneExtension { get; set; } = string.Empty;

}