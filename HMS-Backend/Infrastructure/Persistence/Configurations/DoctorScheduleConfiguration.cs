

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HMS_Backend.Domain.Entities;

namespace HMS_Backend.Infrastructure.Persistence.Configurations;


public class DoctorScheduleConfiguration : IEntityTypeConfiguration<DoctorSchedule>
{
    public void Configure(EntityTypeBuilder<DoctorSchedule> builder)
    {
        builder.HasKey(s => s.Id);
        builder.HasIndex(s => new { s.DoctorId, s.DayOfWeek }).IsUnique();
 
        builder.HasOne(s => s.Doctor)
            .WithMany(d => d.Schedules)
            .HasForeignKey(s => s.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}