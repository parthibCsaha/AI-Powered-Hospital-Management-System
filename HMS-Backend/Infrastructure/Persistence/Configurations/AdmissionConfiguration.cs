
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HMS_Backend.Domain.Entities;

namespace HMS_Backend.Infrastructure.Persistence.Configurations;

public class AdmissionConfiguration : IEntityTypeConfiguration<Admission>
{
    public void Configure(EntityTypeBuilder<Admission> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.AdmissionReason).IsRequired().HasMaxLength(500);
        builder.Property(a => a.TotalBill).HasColumnType("decimal(18,2)");
 
        builder.HasOne(a => a.Patient)
            .WithMany()
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);
 
        builder.HasOne(a => a.Doctor)
            .WithMany()
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);
 
        builder.HasOne(a => a.Room)
            .WithMany(r => r.Admissions)
            .HasForeignKey(a => a.RoomId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}