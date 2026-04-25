
using Clinic_System.Application.Common.Bases;
using Clinic_System.Application.DTOs.Doctors;

namespace Clinic_System.Application.Features.Doctors.Queries.Models
{
    public class GetDoctorWithAppointmentsByIdQuery : IRequest<Response<GetDoctorWithAppointmentDTO>>
    {
        public int Id { get; set; }
    }
}
