



using HMS_Backend.Infrastructure.Services;
using HMS_Backend.Application.Interfaces;
using HMS_Backend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HMS_Backend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // ── Database ──────────────────────────────────────────────────────────
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsqlOptions => npgsqlOptions
                    .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                    .EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorCodesToAdd: null)
            )
        );

        // ── Unit of Work & Repositories ───────────────────────────────────────
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // ── Services ──────────────────────────────────────────────────────────
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;

    }
}