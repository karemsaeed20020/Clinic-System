
using AutoMapper;
using Clinic_System.Application.Common.Bases;
using Clinic_System.Application.DTOs.Doctors;
using Clinic_System.Application.Features.Doctors.Queries.Models;
using Clinic_System.Application.Service.Interface;

namespace Clinic_System.Application.Features.Doctors.Queries.Handlers
{
    public class DoctorListQueryHandler : RsponseHandler, IRequestHandler<GetDoctorListQuery, Response<List<GetDoctorListDTO>>>
    {
        private readonly IDoctorService doctorService;
        private readonly IMapper mapper;
        private readonly ILogger<DoctorListQueryHandler> logger;
        public DoctorListQueryHandler(IDoctorService doctorService,
               IMapper mapper,
               ILogger<DoctorListQueryHandler> logger)
        {
            this.doctorService = doctorService;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<Response<List<GetDoctorListDTO>>> Handle(GetDoctorListQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetDoctorListQuery");
            var doctors = await doctorService.GetDoctorsListAsync(cancellationToken);
            if (doctors?.Any() != true)
            {
                logger.LogWarning("No doctors found");
                return NotFound<List<GetDoctorListDTO>>();
            }
            var doctorsMapper = mapper.Map<List<GetDoctorListDTO>>(doctors);
            logger.LogInformation("Successfully retrieved and mapped doctor list");

            return Success(doctorsMapper);
        }
    }
}
