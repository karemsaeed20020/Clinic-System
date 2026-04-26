
using Clinic_System.Application.Common.Bases;
using Clinic_System.Application.DTOs.Patients;

namespace Clinic_System.Application.Features.Patients.Queries.Models
{
    public class GetPatientWithAppointmentsByIdQuery : IRequest<Response<GetPatientWithAppointmentDTO>>
    {
        public int Id { get; set; }
    }
}
