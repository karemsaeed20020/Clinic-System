using Clinic_System.Application.DTOs.Doctors;
using Clinic_System.Application.Features.Doctors.Commands.Models;
using Clinic_System.Core.Entities;

namespace Clinic_System.Application.Mapping.Doctors
{
    public partial class DoctorProfile
    {
        public void UpdateDoctorMapping()
        {
            // من Command لـ Entity (عشان الحفظ)
            CreateMap<UpdateDoctorCommand, Doctor>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) =>
                     // الشرط المعدل: لا تنقل القيمة إذا كانت null أو فراغ
                     srcMember != null && (!(srcMember is string s) || !string.IsNullOrWhiteSpace(s))
                ));
            CreateMap<Doctor, UpdateDoctorDTO>();
        }
    }
}