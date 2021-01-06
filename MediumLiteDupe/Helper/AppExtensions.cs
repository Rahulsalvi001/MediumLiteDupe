using EntityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace MediumLiteDupe.Helper
{
    public static class AppExtensions
    {
        //public static string GetUserName(this IIdentity identity)
        //{
        //    var claim = ((ClaimsIdentity)identity).FindFirst("Name");
        //    // Test for null to avoid issues during local testing
        //    return (claim != null) ? claim.Value : string.Empty;
        //}
       
    }
    public class MyUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUser, IdentityRole>
    {
        public MyUserClaimsPrincipalFactory(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            var claims = identity.FindAll("FullName").ToList();
            var UserIdClaims = identity.FindAll("ProfileId").ToList();
            if (claims != null && claims.Count > 0)
            {
                foreach (var claim in claims)
                    identity.TryRemoveClaim(claim);
            }
            if (UserIdClaims != null && UserIdClaims.Count > 0)
            {
                foreach (var claim in UserIdClaims)
                    identity.TryRemoveClaim(claim);
            }
            identity.AddClaim(new Claim("FullName", user.Name ?? ""));
            identity.AddClaim(new Claim("ProfileId", user.UserProfileId.HasValue ? user.UserProfileId.Value.ToString() : "0"));
            return identity;
        }
    }
}
