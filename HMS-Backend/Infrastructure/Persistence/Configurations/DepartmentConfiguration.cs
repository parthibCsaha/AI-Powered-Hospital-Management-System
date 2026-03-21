


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HMS_Backend.Domain.Entities;

namespace HMS_Backend.Infrastructure.Persistence.Configurations;


public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Name).IsRequired().HasMaxLength(100);
        builder.HasIndex(d => d.Name).IsUnique();
        builder.Property(d => d.Description).HasMaxLength(500);
        builder.Property(d => d.Location).HasMaxLength(200);
        builder.Property(d => d.PhoneExtension).HasMaxLength(20);
    }
}