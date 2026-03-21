

using HMS_Backend.Application.Interfaces;
using HMS_Backend.Application.Services;
using HMS_Backend.Application.Mappings;
using FluentValidation;


namespace HMS_Backend.Application;


public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly));
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IMedicalRecordService, MedicalRecordService>();
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IAdmissionService, AdmissionService>();
        services.AddScoped<IDashboardService, DashboardService>();

        return services;
    }
}