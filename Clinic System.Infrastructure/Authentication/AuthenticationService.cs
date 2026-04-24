namespace Clinic_System.Infrastructure.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthenticationService(IOptions<JwtSettings> jwtSettings, UserManager<ApplicationUser> userManager)
        {
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
        }
        public async Task<(string AccessToken, string RefreshToken, DateTime ExpiresAt, string? userName, string? email, List<string>? Roles)> GenerateJwtTokenAsync(string userId, string userName, string email, List<string> roles, List<Claim>? extraClaim = null)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.UniqueName, userName),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            if (extraClaim != null)
            {
                authClaims.AddRange(extraClaim);
            }
            var expiresAt = DateTime.Now.AddMinutes(_jwtSettings.TokenExpirationInMinutes);

            var tokenObject = new JwtSecurityToken(
                issuer: _jwtSettings.IssuerIP,
                audience: _jwtSettings.AudienceIP,
                expires: expiresAt,
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
             );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenObject);
            var refreshToken = GenerateRefreshToken();

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.RefreshTokens ??= new List<RefreshToken>();
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }
            return (accessToken, refreshToken.Token, expiresAt, userName, email, roles);
        }

        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new Byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpirationInDays),
                CreatedOn = DateTime.Now,
            };
        }
    }
}
