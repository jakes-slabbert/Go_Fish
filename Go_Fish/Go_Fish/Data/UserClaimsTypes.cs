

using Microsoft.AspNetCore.Identity;

namespace GoFish.Data
{
    public class UserClaimsTypes : IdentityUserClaim<int>
    {
        /// <summary>
        /// Id of the User which started the Impersonation
        /// </summary>
        public const string ImpId = "ImpId";
    }
}
