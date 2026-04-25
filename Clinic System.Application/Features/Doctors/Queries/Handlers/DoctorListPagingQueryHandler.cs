
using AutoMapper;
using Clinic_System.Application.Common;
using Clinic_System.Application.Common.Bases;
using Clinic_System.Application.DTOs.Doctors;
using Clinic_System.Application.Features.Doctors.Queries.Models;
using Clinic_System.Application.Service.Interface;

namespace Clinic_System.Application.Features.Doctors.Queries.Handlers
{
    public class DoctorListPagingQueryHandler : RsponseHandler, IRequestHandler<GetDoctorListPagingQuery, Response<PagedResult<GetDoctorListDTO>>>
    {
        private readonly IDoctorService doctorService;
        private readonly IMapper mapper;
        private readonly ICacheService cacheService;
        private readonly ILogger<DoctorListPagingQueryHandler> logger;

        public DoctorListPagingQueryHandler(
            IDoctorService doctorService,
            IMapper mapper,
            ILogger<DoctorListPagingQueryHandler> logger,
            ICacheService cacheService)
        {
            this.doctorService = doctorService;
            this.mapper = mapper;
            this.logger = logger;
            this.cacheService = cacheService;
        }
        public async Task<Response<PagedResult<GetDoctorListDTO>>> Handle(GetDoctorListPagingQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"DoctorsList_Page_{request.PageNumber}_Size_{request.PageSize}";

            var cachedDoctors = await cacheService.GetDataAsync<PagedResult<GetDoctorListDTO>>(cacheKey);

            if (cachedDoctors != null)
            {
                logger.LogInformation("Successfully retrieved doctors from CACHE for {CacheKey}", cacheKey);
                return Success(cachedDoctors);
            }


            var doctors = await doctorService.GetDoctorsListPagingAsync(request.PageNumber, request.PageSize, cancellationToken);

            if (doctors?.Items.Any() != true)
            {
                logger.LogWarning("No doctors found for PageNumber={PageNumber}, PageSize={PageSize}", request.PageNumber, request.PageSize);
                return NotFound<PagedResult<GetDoctorListDTO>>();
            }

            var doctorsMapper = mapper.Map<List<GetDoctorListDTO>>(doctors.Items);
            var pagedResult = new PagedResult<GetDoctorListDTO>(doctorsMapper, doctors.TotalCount, doctors.CurrentPage, doctors.PageSize);

            logger.LogInformation("Successfully retrieved {Count} doctors for PageNumber={PageNumber}, PageSize={PageSize}", doctors.Items.Count(), request.PageNumber, request.PageSize);

            await cacheService.SetDataAsync(cacheKey, pagedResult, TimeSpan.FromMinutes(30));

            return Success(pagedResult);
        }
    }
}
