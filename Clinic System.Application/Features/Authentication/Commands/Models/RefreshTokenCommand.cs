
using Clinic_System.Application.Common.Bases;
using Clinic_System.Application.DTOs.Authentications;

namespace Clinic_System.Application.Features.Authentication.Commands.Models
{
    public class RefreshTokenCommand : IRequest<Response<JwtAuthResult>>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
