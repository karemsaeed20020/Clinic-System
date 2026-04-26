
using Clinic_System.Application.Common.Bases;
using Clinic_System.Application.DTOs.Patients;

namespace Clinic_System.Application.Features.Patients.Queries.Models
{
    public class GetPatientListByNameQuery : IRequest<Response<List<GetPatientListDTO>>>
    {
        public string FullName { get; set; } = null!;
    }
}
