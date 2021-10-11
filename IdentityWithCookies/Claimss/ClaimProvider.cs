using IdentityWithCookies.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityWithCookies.Claimss
{
    public class ClaimProvider : IClaimsTransformation
    {
        UserManager<User> _userManager;

        public ClaimProvider(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            ClaimsIdentity identity = principal.Identity as ClaimsIdentity;
            if (principal != null && principal.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(principal.Identity.Name);
                if (user != null)
                {
                    if (user.City != null)
                    {
                        if (!principal.HasClaim(i => i.Type == "City"))
                        {

                            Claim newClaim = new Claim("City", user.City, ClaimValueTypes.String, "Internal");
                            identity.AddClaim(newClaim); 
                        }
                    }
                }
            }
            return principal;
        }
    }
}
