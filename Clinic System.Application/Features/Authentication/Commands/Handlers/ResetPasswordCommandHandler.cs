
using Clinic_System.Application.Common.Bases;
using Clinic_System.Application.Features.Authentication.Commands.Models;
using Clinic_System.Application.Service.Interface;

namespace Clinic_System.Application.Features.Authentication.Commands.Handlers
{
    public class ResetPasswordCommandHandler : RsponseHandler, IRequestHandler<ResetPasswordCommand, Response<string>>
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<ResetPasswordCommandHandler> _logger;

        public ResetPasswordCommandHandler(IIdentityService identityService, ILogger<ResetPasswordCommandHandler> logger)
        {
            _identityService = identityService;
            _logger = logger;
        }

        public async Task<Response<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var decodedCode = _identityService.DecodeToken(request.Code);

                var (succeeded, error) = await _identityService.ResetPasswordAsync(request.Email, decodedCode, request.NewPassword);

                if (succeeded)
                {
                    _logger.LogInformation("Password reset successfully for user: {Email}", request.Email);
                    return Success("Password has been changed successfully.");
                }
                return BadRequest<string>(error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting password for email: {Email}", request.Email);
                return Failure<string>("An unexpected error occurred.");
            }
        }
    }
}
