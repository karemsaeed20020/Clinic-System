
using AutoMapper;
using Clinic_System.Application.Common.Bases;
using Clinic_System.Application.DTOs.Doctors;
using Clinic_System.Application.Features.Doctors.Queries.Models;
using Clinic_System.Application.Service.Interface;

namespace Clinic_System.Application.Features.Doctors.Queries.Handlers
{
    public class DoctorWithAppointmentsByIdQueryHandler : AppRequestHandler<GetDoctorWithAppointmentsByIdQuery, GetDoctorWithAppointmentDTO>
    {
        private readonly IDoctorService doctorService;
        private readonly IMapper mapper;
        private readonly IIdentityService identityService;
        private readonly ICacheService cacheService;
        private readonly ILogger<DoctorWithAppointmentsByIdQueryHandler> logger;

        public DoctorWithAppointmentsByIdQueryHandler(
            ICurrentUserService currentUserService,
            IDoctorService doctorService,
            IMapper mapper,
            IIdentityService identityService,
            ICacheService cacheService,
            ILogger<DoctorWithAppointmentsByIdQueryHandler> logger) : base(currentUserService)
        {
            this.doctorService = doctorService;
            this.mapper = mapper;
            this.identityService = identityService;
            this.cacheService = cacheService;
            this.logger = logger;
        }

        public override async Task<Response<GetDoctorWithAppointmentDTO>> Handle(GetDoctorWithAppointmentsByIdQuery request, CancellationToken cancellationToken)
        {
            var authResult = await ValidateDoctorAccess(request.Id);
            if (authResult != null)
                return authResult;

            var cacheKey = $"DoctorWithAppointmentsById:{request.Id}";

            var cachedDoctor = await cacheService.GetDataAsync<GetDoctorWithAppointmentDTO>(cacheKey);

            if (cachedDoctor != null)
            {
                logger.LogInformation("Successfully retrieved doctor with appointments from CACHE for {CacheKey}", cacheKey);
                return Success(cachedDoctor);
            }

            var doctor = await doctorService.GetDoctorWithAppointmentsByIdAsync(request.Id, cancellationToken);

            if (doctor == null)
            {
                logger.LogInformation("GetDoctorWithAppointmentsByIdQueryHandler: Doctor with ID {Id} not found", request.Id);
                return NotFound<GetDoctorWithAppointmentDTO>($"Doctor with ID {request.Id} not found");
            }

            var doctorsMapper = mapper.Map<GetDoctorWithAppointmentDTO>(doctor);

            if (!string.IsNullOrEmpty(doctor.ApplicationUserId))
            {
                var (email, userName) = await identityService.GetUserEmailAndUserNameAsync(doctor.ApplicationUserId, cancellationToken);

                doctorsMapper.Email = email;
                doctorsMapper.UserName = userName;

                if (string.IsNullOrEmpty(doctorsMapper.Email) || string.IsNullOrEmpty(doctorsMapper.UserName))
                {
                    logger.LogWarning("Missing Identity data for Doctor AppUserId: {AppUserId}", doctor.ApplicationUserId);
                }
            }

            logger.LogInformation("Successfully retrieved doctor with appointments, ID: {Id}", request.Id);

            await cacheService.SetDataAsync(cacheKey, doctorsMapper, TimeSpan.FromMinutes(10));

            return Success(doctorsMapper);
        }
    }
}
