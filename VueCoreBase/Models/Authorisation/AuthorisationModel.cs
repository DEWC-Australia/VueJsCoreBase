using ASPIdentity.Data;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Models.Authorisation
{
    public class AuthorisationModel
    {
        public static async Task<IdentityResult> BuildCustomer(UserManager<ApplicationUser> userManager, ApplicationUser user)
        {
            return await userManager.AddToRoleAsync(user, AuthorisationConstants.Roles.Customer);
        }

        public static async Task<IdentityResult> BuildEmployee(UserManager<ApplicationUser> userManager, ApplicationUser user)
        {
            return await userManager.AddToRoleAsync(user, AuthorisationConstants.Roles.Employee);
        }

        public static async Task<IdentityResult> BuildAdmin(UserManager<ApplicationUser> userManager, ApplicationUser user)
        {
            return await userManager.AddToRoleAsync(user, AuthorisationConstants.Roles.Admin);
        }

    }
}
