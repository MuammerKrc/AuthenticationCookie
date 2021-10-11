using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWithCookies.Identity
{
    public class User:IdentityUser
    {
        public string  City { get; set; }
        public string PictureUrl { get; set; }
        public DateTime? BirthDay { get; set; }
        public int Gender { get; set; }

    }
}
