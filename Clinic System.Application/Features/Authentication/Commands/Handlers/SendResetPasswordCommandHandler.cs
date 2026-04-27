
using Clinic_System.Application.Common;
using Clinic_System.Application.Common.Bases;
using Clinic_System.Application.Features.Authentication.Commands.Models;
using Clinic_System.Application.Service.Interface;

namespace Clinic_System.Application.Features.Authentication.Commands.Handlers
{
    public class SendResetPasswordCommandHandler : RsponseHandler, IRequestHandler<SendResetPasswordCommand, Response<string>>
    {
        private readonly IIdentityService _identityService;
        private readonly IEmailService _emailService;
        private readonly ILogger<SendResetPasswordCommandHandler> _logger;

        public SendResetPasswordCommandHandler(
            IIdentityService identityService,
            IEmailService emailService,
            ILogger<SendResetPasswordCommandHandler> logger)
        {
            _identityService = identityService;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<Response<string>> Handle(SendResetPasswordCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Password reset requested for email: {Email}", request.Email);

            try
            {
                var token = await _identityService.GeneratePasswordResetTokenAsync(request.Email);

                var encodedToken = _identityService.EncodeToken(token);

                var resetLink = $"{request.BaseUrl}/api/authentication/reset-password?email={request.Email}&code={encodedToken}";

                var emailBody = EmailTemplates.GetResetPasswordTemplate(request.Email, resetLink);

                await _emailService.SendEmailAsync(request.Email, "Elite Clinic - Password Reset Request", emailBody);

                _logger.LogInformation("Password reset email sent successfully to: {Email}", request.Email);

                return Success("Password reset link has been sent to your email.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling password reset for email: {Email}", request.Email);
                return BadRequest<string>("Failed to send reset email. Please try again later or check the email address.");
            }
        }
    }
}
