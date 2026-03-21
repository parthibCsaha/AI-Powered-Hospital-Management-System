

using HMS_Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS_Backend.Infrastructure.Persistence.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(p => p.LastName).IsRequired().HasMaxLength(50);
        builder.Property(p => p.Email).IsRequired().HasMaxLength(100);
        builder.HasIndex(p => p.Email).IsUnique();
        builder.Property(p => p.PhoneNumber).IsRequired().HasMaxLength(20);
        builder.Property(p => p.Address).IsRequired().HasMaxLength(250);
        builder.Property(p => p.Gender).HasConversion<string>();
        builder.Property(p => p.BloodGroup).HasConversion<string>();
        builder.Ignore(p => p.FullName);
        builder.Ignore(p => p.Age);
    }
}