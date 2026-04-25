using Clinic_System.Application.Common.Bases;

namespace Clinic_System.Application.Features.Patients.Commands.Models
{
    public class HardDeletePatientCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
    }
}
