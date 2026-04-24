
using Clinic_System.Application.Common.Bases;
using Clinic_System.Application.DTOs.Doctors;
using Clinic_System.Application.Features.Doctors.Commands.Models;

namespace Clinic_System.Application.Features.Doctors.Commands.Handlers
{
    public class CreateDoctorCommandHandler : IRequestHandler<CreateDoctorCommand, Response<CreateDoctorDTO>>
    {
        public CreateDoctorCommandHandler()
        {

        }
        public Task<Response<CreateDoctorDTO>> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
