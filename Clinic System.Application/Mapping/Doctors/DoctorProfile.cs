
using AutoMapper;

namespace Clinic_System.Application.Mapping.Doctors
{
    public partial class DoctorProfile : Profile
    {
        public DoctorProfile()
        {
            GetDoctorListMapping();
            GetDoctorByIdMapping();
            GetDoctorBasicInfoMapping();
            GetDoctorWithAppointmentsByIdMapping();
            CreateDoctorMapping();
            UpdateDoctorMapping();
        }
    }
}
