

using GoFish.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace GoFishHelpers.Identity
{
    public class AppUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUser, AppRole>
    {
        public AppUserClaimsPrincipalFactory(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {
        }

        /// <summary>
        /// This is Claims which we need to add specifically for the customers project by default the DisplayName is added by the framework
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            var appUser = user as AppUser;

            // Add Extra Claim data here which you might want to use later on
            //identity.AddClaim(new Claim(UserClaimsTypes.CompanyId, user.CompanyId.ToString()));

            return identity;
        }
    }
}
