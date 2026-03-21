using HMS_Backend.API.Extensions;
using HMS_Backend.API.Middleware;
using HMS_Backend.Application;
using HMS_Backend.Infrastructure;
using HMS_Backend.Infrastructure.Persistence;
using HMS_Backend.Infrastructure.Persistence.Seeding;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting HMS Backend API...");

    var builder = WebApplication.CreateBuilder(args);

    // Serilog
    builder.Host.UseSerilog((ctx, lc) => lc
        .ReadFrom.Configuration(ctx.Configuration)
        .WriteTo.Console(outputTemplate:
            "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
        .WriteTo.File("logs/hms-.log",
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 30)
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithEnvironmentName());

    // Layers
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    // API
    builder.Services.AddControllers()
        .ConfigureApiBehaviorOptions(o => o.SuppressModelStateInvalidFilter = true);

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerWithJwt();
    builder.Services.AddJwtAuthentication(builder.Configuration);
    builder.Services.AddCorsPolicy();
    builder.Services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>("database");
    builder.Services.AddRouting(options => options.LowercaseUrls = true); // Enforce lowercase URLs

    var app = builder.Build();

    // Seed
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger  = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        await DataSeeder.SeedAsync(context, logger);
    }

    // Middleware pipeline
    app.UseMiddleware<GlobalExceptionMiddleware>();
    app.UseMiddleware<RequestLoggingMiddleware>();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HMS API v1");
        c.RoutePrefix = string.Empty;
        c.DisplayRequestDuration();
    });

    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.UseCors("HmsPolicy");
    app.UseAuthentication();
    app.UseAuthorization();
 
    app.MapControllers();
    app.MapHealthChecks("/health");
 
    await app.RunAsync();

}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "HMS API terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}
