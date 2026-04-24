
namespace Clinic_System.Infrastructure.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthorizationService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<string> AddRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return "User Not Found";

            if (!await IsRoleExistAsync(roleName))
            {
                return "Role Not Found";
            }


            return "Failed";
        }

        public async Task<bool> IsRoleExistAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }
    }
}
