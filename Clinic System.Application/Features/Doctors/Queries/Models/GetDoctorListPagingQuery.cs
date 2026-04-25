using Clinic_System.Application.Common;
using Clinic_System.Application.Common.Bases;
using Clinic_System.Application.DTOs.Doctors;

namespace Clinic_System.Application.Features.Doctors.Queries.Models
{
    public class GetDoctorListPagingQuery : IRequest<Response<PagedResult<GetDoctorListDTO>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
