
using Clinic_System.Application.Common;
using Clinic_System.Application.Common.Bases;
using Clinic_System.Application.DTOs.Patients;

namespace Clinic_System.Application.Features.Patients.Queries.Models
{
    public class GetPatientListPagingQuery : IRequest<Response<PagedResult<GetPatientListDTO>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
