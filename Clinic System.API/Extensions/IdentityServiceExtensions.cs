
namespace Clinic_System.API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdenityServices(this IServiceCollection services, IConfiguration configuration)
        {
            // ==========================================
            // 1. Identity Configuration
            // ==========================================

            services.AddIdentity<ApplicationUser, Microsoft.AspNetCore.Identity.IdentityRole>(options =>
            {
                // Password Settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;

                // User Settings
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;

                // Lockout Settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            // ==========================================
            // 2. JWT Configuration & Validation Checks
            // ==========================================

            var jwtSettings = configuration.GetSection("JWT").Get<JwtSettings>();
            if (jwtSettings == null || string.IsNullOrWhiteSpace(jwtSettings.SecretKey))
                throw new Exception("JWT SecretKey is missing in appsettings.json");

            if (string.IsNullOrWhiteSpace(jwtSettings.IssuerIP))
                throw new Exception("JWT IssuerIP is missing in appsettings.json");

            if (string.IsNullOrWhiteSpace(jwtSettings.AudienceIP))
                throw new Exception("JWT AudienceIP is missing in appsettings.json");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.IssuerIP,
                        ValidAudience = jwtSettings.AudienceIP,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                    };
                });


            return services;
        }
    }
}
