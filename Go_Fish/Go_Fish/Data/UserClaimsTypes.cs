

using Microsoft.AspNetCore.Identity;

namespace GoFishData
{
    public class UserClaimsTypes : IdentityUserClaim<int>
    {
        /// <summary>
        /// Id of the User which started the Impersonation
        /// </summary>
        public const string ImpId = "ImpId";
    }
}
