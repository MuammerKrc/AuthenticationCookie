using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWithCookies.Identity
{
    public class Role:IdentityRole
    {
        public string RoleType { get; set; }
    }
}
