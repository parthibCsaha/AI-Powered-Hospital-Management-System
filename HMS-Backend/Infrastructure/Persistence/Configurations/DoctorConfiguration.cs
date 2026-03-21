


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HMS_Backend.Domain.Entities;

namespace HMS_Backend.Infrastructure.Persistence.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(d => d.LastName).IsRequired().HasMaxLength(50);
        builder.Property(d => d.Email).IsRequired().HasMaxLength(100);
        builder.HasIndex(d => d.Email).IsUnique();
        builder.Property(d => d.LicenseNumber).IsRequired().HasMaxLength(50);
        builder.HasIndex(d => d.LicenseNumber).IsUnique();
        builder.Property(d => d.Specialization).IsRequired().HasMaxLength(100);
        builder.Property(d => d.ConsultationFee).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(d => d.Gender).HasConversion<string>();
        builder.Ignore(d => d.FullName);
 
        builder.HasOne(d => d.Department)
            .WithMany(dep => dep.Doctors)
            .HasForeignKey(d => d.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}