

using HMS_Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using HMS_Backend.Domain.Enums;
using HMS_Backend.Application.Interfaces;

namespace HMS_Backend.Infrastructure.Persistence.Seeding;

public static class DataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, ILogger logger)
    {
        try
        {
            await context.Database.MigrateAsync();

            await SeedDepartmentsAsync(context, logger);
            await SeedRoomsAsync(context, logger);
            await SeedAdminUserAsync(context, logger);

            logger.LogInformation("Database seeding completed successfully.");

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database: {Message}", ex.Message);
            throw;
        }
    }

    private static async Task SeedDepartmentsAsync(ApplicationDbContext context, ILogger logger)
    {
        if (await context.Departments.AnyAsync()) return;
 
        var departments = new List<Department>
        {
            new() { Name = "Cardiology",       Description = "Heart and cardiovascular system care.",  Location = "Block A, Floor 1", PhoneExtension = "1001" },
            new() { Name = "Neurology",        Description = "Disorders of the nervous system.",       Location = "Block A, Floor 2", PhoneExtension = "1002" },
            new() { Name = "Orthopedics",      Description = "Bones, joints, and muscle disorders.",   Location = "Block B, Floor 1", PhoneExtension = "1003" },
            new() { Name = "Pediatrics",       Description = "Medical care for children.",             Location = "Block B, Floor 2", PhoneExtension = "1004" },
            new() { Name = "General Surgery",  Description = "Surgical procedures across the body.",  Location = "Block C, Floor 1", PhoneExtension = "1005" },
            new() { Name = "Oncology",         Description = "Cancer diagnosis and treatment.",        Location = "Block C, Floor 2", PhoneExtension = "1006" },
            new() { Name = "Emergency",        Description = "Emergency and trauma care.",             Location = "Block D, Floor 0", PhoneExtension = "1007" },
            new() { Name = "Radiology",        Description = "Medical imaging and diagnostics.",       Location = "Block D, Floor 1", PhoneExtension = "1008" },
            new() { Name = "Dermatology",      Description = "Skin, hair, and nail disorders.",        Location = "Block E, Floor 1", PhoneExtension = "1009" },
            new() { Name = "Ophthalmology",    Description = "Eye care and vision disorders.",         Location = "Block E, Floor 2", PhoneExtension = "1010" },
        };
 
        await context.Departments.AddRangeAsync(departments);
        await context.SaveChangesAsync();
        logger.LogInformation("Seeded {Count} departments.", departments.Count);
    }

    private static async Task SeedRoomsAsync(ApplicationDbContext context, ILogger logger)
    {
        if (await context.Rooms.AnyAsync()) return;
 
        var rooms = new List<Room>();
 
        // General wards — Floor 1
        for (int i = 1; i <= 10; i++)
            rooms.Add(new Room { RoomNumber = $"G{i:D2}", RoomType = RoomType.GeneralWard, Floor = 1, DailyRate = 500m });
 
        // Private rooms — Floor 2
        for (int i = 1; i <= 8; i++)
            rooms.Add(new Room { RoomNumber = $"P{i:D2}", RoomType = RoomType.Private, Floor = 2, DailyRate = 1500m });
 
        // ICU — Floor 3
        for (int i = 1; i <= 6; i++)
            rooms.Add(new Room { RoomNumber = $"ICU{i:D2}", RoomType = RoomType.ICU, Floor = 3, DailyRate = 5000m });
 
        // Operating Theatres — Floor 4
        for (int i = 1; i <= 4; i++)
            rooms.Add(new Room { RoomNumber = $"OT{i:D2}", RoomType = RoomType.OperatingTheatre, Floor = 4, DailyRate = 10000m });
 
        await context.Rooms.AddRangeAsync(rooms);
        await context.SaveChangesAsync();
        logger.LogInformation("Seeded {Count} rooms.", rooms.Count);
    }
 
    private static async Task SeedAdminUserAsync(ApplicationDbContext context, ILogger logger)
    {
        if (await context.Users.AnyAsync(u => u.Role == UserRole.Admin)) return;
 
        var adminUser = new User
        {
            Email = "admin@hospital.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@1234"),
            FirstName = "System",
            LastName = "Admin",
            Role = UserRole.Admin,
            IsActive = true
        };
 
        await context.Users.AddAsync(adminUser);
        await context.SaveChangesAsync();
        logger.LogInformation("Seeded default admin user: {Email}", adminUser.Email);
    }

}