
using Clinic_System.Application.Common.Bases;
using Clinic_System.Application.DTOs.Patients;

namespace Clinic_System.Application.Features.Patients.Queries.Models
{
    public class GetPatientByPhoneQuery : IRequest<Response<GetPatientDTO>>
    {
        public string Phone { get; set; } = null!;
    }
}
