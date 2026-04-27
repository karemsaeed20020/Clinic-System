
using Clinic_System.Application.Common.Bases;
using Clinic_System.Application.Features.Authentication.Queries.Models;
using Clinic_System.Application.Service.Interface;

namespace Clinic_System.Application.Features.Authentication.Queries.Handlers
{
    public class ConfirmEmailQueryHandler : RsponseHandler, IRequestHandler<ConfirmEmailQuery, Response<string>>
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<ConfirmEmailQueryHandler> _logger;

        public ConfirmEmailQueryHandler(IIdentityService identityService, ILogger<ConfirmEmailQueryHandler> logger)
        {
            _identityService = identityService;
            _logger = logger;
        }

        public async Task<Response<string>> Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to confirm email for user ID: {UserId}", request.UserId);

            try
            {
                var decodedCode = _identityService.DecodeToken(request.Code);


                var isConfirmed = await _identityService.ConfirmEmailAsync(request.UserId, decodedCode);

                if (isConfirmed)
                {
                    _logger.LogInformation("Email confirmed successfully for User ID: {UserId}", request.UserId);
                    return Success("Email confirmed successfully! You can now login.");
                }

                _logger.LogWarning("Email confirmation failed for User ID: {UserId}", request.UserId);
                return BadRequest<string>("Invalid user ID or confirmation code.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while confirming email for User ID: {UserId}", request.UserId);
                return Failure<string>("An unexpected error occurred during email confirmation.");
            }
        }
    }
}
