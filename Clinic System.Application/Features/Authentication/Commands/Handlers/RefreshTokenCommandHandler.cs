
using Clinic_System.Application.Common.Bases;
using Clinic_System.Application.DTOs.Authentications;
using Clinic_System.Application.Features.Authentication.Commands.Models;
using Clinic_System.Application.Service.Interface;
using Clinic_System.Core.Interfaces.UnitOfWork;

namespace Clinic_System.Application.Features.Authentication.Commands.Handlers
{
    public class RefreshTokenCommandHandler : RsponseHandler, IRequestHandler<RefreshTokenCommand, Response<JwtAuthResult>>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<RefreshTokenCommandHandler> _logger;

        public RefreshTokenCommandHandler(IAuthenticationService authenticationService, IUnitOfWork unitOfWork, ILogger<RefreshTokenCommandHandler> logger)
        {
            _authenticationService = authenticationService;
            this.unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Response<JwtAuthResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start handling RefreshTokenCommand for AccessToken: {AccessToken}", request.AccessToken);

            try
            {
                var principal = _authenticationService.GetPrincipalFromExpiredToken(request.AccessToken);

                if (principal == null)
                    return BadRequest<JwtAuthResult>("Invalid Token");

                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;


                var customClaims = new List<Claim>();

                if (principal.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Doctor"))
                {
                    var doctor = await unitOfWork.DoctorsRepository.GetDoctorByUserIdAsync(userId);
                    if (doctor != null)
                    {
                        customClaims.Add(new Claim("DoctorId", doctor.Id.ToString()));
                    }
                }
                else if (principal.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Patient"))
                {
                    var patient = await unitOfWork.PatientsRepository.GetPatientByUserIdAsync(userId);
                    if (patient != null)
                    {
                        customClaims.Add(new Claim("PatientId", patient.Id.ToString()));
                    }
                }

                var (accessToken, refreshToken, expiresAt) = await _authenticationService.RefreshTokenAsync(request.AccessToken, request.RefreshToken, customClaims);

                if (string.IsNullOrEmpty(accessToken))
                {
                    _logger.LogWarning("Failed to refresh token. Invalid or expired refresh token provided.");
                    return Unauthorized<JwtAuthResult>("Invalid or Expired Refresh Token");
                }

                var response = new JwtAuthResult
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = expiresAt.ToString("yyyy-MM-dd HH:mm:ss")
                };

                _logger.LogInformation("Token refreshed successfully for user.");

                return Success(response, "Token Refreshed Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while refreshing the token.");

                return Unauthorized<JwtAuthResult>("An error occurred while processing your request.");
            }
        }
    }
}
