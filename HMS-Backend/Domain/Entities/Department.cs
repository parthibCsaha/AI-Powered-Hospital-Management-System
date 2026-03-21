using HMS_Backend.Domain.Common;

namespace HMS_Backend.Domain.Entities;

public class Department : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string PhoneExtension { get; set; } = string.Empty;
 
    // Navigation
    public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    
}
