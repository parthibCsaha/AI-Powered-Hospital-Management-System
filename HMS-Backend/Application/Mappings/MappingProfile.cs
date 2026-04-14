
using AutoMapper;
using HMS_Backend.Application.DTOs.Patient;
using HMS_Backend.Application.DTOs.Doctor;
using HMS_Backend.Application.DTOs.Department;
using HMS_Backend.Application.DTOs.Appointment;
using HMS_Backend.Application.DTOs.MedicalRecord;
using HMS_Backend.Application.DTOs.Room;
using HMS_Backend.Application.DTOs.Admission;
using HMS_Backend.Domain.Entities;


namespace HMS_Backend.Application.Mappings;


public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Patient
        // <Source, Destination>
        CreateMap<Patient, PatientResponseDto>()
            .ForMember(d => d.FullName, o => o.MapFrom(s => s.FullName))
            .ForMember(d => d.Age, o => o.MapFrom(s => s.Age))
            .ForMember(d => d.Gender, o => o.MapFrom(s => s.Gender.ToString()))
            .ForMember(d => d.BloodGroup, o => o.MapFrom(s => s.BloodGroup.ToString()));
    
        CreateMap<Patient, PatientSummaryDto>()
            .ForMember(d => d.FullName, o => o.MapFrom(s => s.FullName))
            .ForMember(d => d.Age, o => o.MapFrom(s => s.Age))
            .ForMember(d => d.Gender, o => o.MapFrom(s => s.Gender.ToString()))
            .ForMember(d => d.BloodGroup, o => o.MapFrom(s => s.BloodGroup.ToString()));

        CreateMap<CreatePatientDto, Patient>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore())
            .ForMember(d => d.IsDeleted, o => o.Ignore());
        CreateMap<UpdatePatientDto, Patient>();

        // Doctor
        CreateMap<Doctor, DoctorResponseDto>()
            .ForMember(d => d.FullName, o => o.MapFrom(s => s.FullName))
            .ForMember(d => d.Gender, o => o.MapFrom(s => s.Gender.ToString()))
            .ForMember(d => d.DepartmentName, o => o.MapFrom(s => s.Department != null ? s.Department.Name : string.Empty));
 
        CreateMap<Doctor, DoctorSummaryDto>()
            .ForMember(d => d.FullName, o => o.MapFrom(s => s.FullName))
            .ForMember(d => d.DepartmentName, o => o.MapFrom(s => s.Department != null ? s.Department.Name : string.Empty));
 
        CreateMap<CreateDoctorDto, Doctor>();
        CreateMap<UpdateDoctorDto, Doctor>();
 
        CreateMap<DoctorSchedule, DoctorScheduleDto>();
        CreateMap<DoctorScheduleDto, DoctorSchedule>();

        // Department
        CreateMap<Department, DepartmentResponseDto>()
            .ForMember(d => d.DoctorCount, o => o.MapFrom(s => s.Doctors != null ? s.Doctors.Count(d => !d.IsDeleted) : 0));
        CreateMap<CreateDepartmentDto, Department>();
        CreateMap<UpdateDepartmentDto, Department>();

        // Appointment
        CreateMap<Appointment, AppointmentResponseDto>()
            .ForMember(d => d.PatientName, o => o.MapFrom(s => s.Patient != null ? s.Patient.FullName : string.Empty))
            .ForMember(d => d.DoctorName, o => o.MapFrom(s => s.Doctor != null ? s.Doctor.FullName : string.Empty))
            .ForMember(d => d.DoctorSpecialization, o => o.MapFrom(s => s.Doctor != null ? s.Doctor.Specialization : string.Empty))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
 
        CreateMap<CreateAppointmentDto, Appointment>();
        CreateMap<UpdateAppointmentDto, Appointment>();
 
        // MedicalRecord
        CreateMap<MedicalRecord, MedicalRecordResponseDto>()
            .ForMember(d => d.PatientName, o => o.MapFrom(s => s.Patient != null ? s.Patient.FullName : string.Empty))
            .ForMember(d => d.DoctorName, o => o.MapFrom(s => s.Doctor != null ? s.Doctor.FullName : string.Empty));
 
        CreateMap<CreateMedicalRecordDto, MedicalRecord>();
        CreateMap<UpdateMedicalRecordDto, MedicalRecord>();
 
        // Room
        CreateMap<Room, RoomResponseDto>();
        CreateMap<CreateRoomDto, Room>();
        CreateMap<UpdateRoomDto, Room>();

        // Admission
        CreateMap<Admission, AdmissionResponseDto>()
            .ForMember(d => d.PatientName, o => o.MapFrom(s => s.Patient != null ? s.Patient.FullName : string.Empty))
            .ForMember(d => d.DoctorName, o => o.MapFrom(s => s.Doctor != null ? s.Doctor.FullName : string.Empty))
            .ForMember(d => d.RoomNumber, o => o.MapFrom(s => s.Room != null ? s.Room.RoomNumber : string.Empty))
            .ForMember(d => d.RoomType, o => o.MapFrom(s => s.Room != null ? s.Room.RoomType.ToString() : string.Empty))
            .ForMember(d => d.DaysAdmitted, o => o.MapFrom(s => s.DischargeDate.HasValue
                    ? (int)(s.DischargeDate.Value - s.AdmissionDate).TotalDays
                    : (int)(DateTime.UtcNow - s.AdmissionDate).TotalDays));
 
        CreateMap<CreateAdmissionDto, Admission>();

    }
}
