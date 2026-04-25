using Clinic_System.Application.Common.Bases;
using Clinic_System.Application.DTOs.Doctors;

namespace Clinic_System.Application.Features.Doctors.Queries.Models
{
    public class GetDoctorListBySpecializationQuery : IRequest<Response<List<GetDoctorBasicInfoDTO>>>
    {
        public string Specialization { get; set; } = null!;
    }
}
