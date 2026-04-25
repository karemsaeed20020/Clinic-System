
using Clinic_System.Application.Common.Bases;
using Clinic_System.Core.Entities;

namespace Clinic_System.Application.Features.Doctors.Commands.Models
{
    public class SoftDeleteDoctorCommand : IRequest<Response<Doctor>>
    {
        public int Id { get; set; }
    }
}
