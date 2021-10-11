using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWithCookies.CustomValidator
{
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError
            {
                Code = "InvalidUserName",
                Description = $"Bu {userName} geçersizdir"
            };
        }
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError()
            {
                Code = "DuplicateEmail",
                Description = $"Bu {email} kullanılmaktadır"
            };
        }
    }
}
