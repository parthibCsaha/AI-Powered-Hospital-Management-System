

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HMS_Backend.Domain.Entities;

namespace HMS_Backend.Infrastructure.Persistence.Configurations;


public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.RoomNumber).IsRequired().HasMaxLength(20);
        builder.HasIndex(r => r.RoomNumber).IsUnique();
        builder.Property(r => r.RoomType).IsRequired().HasMaxLength(50);
        builder.Property(r => r.DailyRate).HasColumnType("decimal(18,2)");
    }
}