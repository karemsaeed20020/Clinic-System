using Clinic_System.Application.DTOs.Patients;
using Clinic_System.Application.Features.Patients.Commands.Models;
using Clinic_System.Core.Entities;

namespace Clinic_System.Application.Mapping.Patients
{
    public partial class PatientProfile
    {
        public void CreatePatientMapping()
        {
            // من Command لـ Entity (عشان الحفظ)
            CreateMap<CreatePatientCommand, Patient>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.ApplicationUserId, opt => opt.Ignore())
                .ForMember(dest => dest.Appointments, opt => opt.Ignore());

            CreateMap<Patient, CreatePatientDTO>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))

                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToString("dd/MM/yyyy")))

                .ForMember(dest => dest.CreatedAt, option => option.MapFrom(src => src.CreatedAt.ToString("dd/MM/yyyy-hh:mm")));
        }
    }
}