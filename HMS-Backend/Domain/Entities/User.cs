using HMS_Backend.Domain.Common;
using HMS_Backend.Domain.Enums;

namespace HMS_Backend.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime? LastLoginAt { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
 
    // Optional link to doctor or patient profile
    public Guid? DoctorId { get; set; }
    public Guid? PatientId { get; set; }
    
}