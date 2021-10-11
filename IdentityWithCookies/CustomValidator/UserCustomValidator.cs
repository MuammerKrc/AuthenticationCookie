using IdentityWithCookies.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWithCookies.CustomValidator
{
    public class UserCustomValidator : IUserValidator<User>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
        {
            List<IdentityError> errors = new List<IdentityError>();

            string[] digits = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            foreach (var item in digits)
            {
                if (user.UserName.StartsWith(item))
                {
                    errors.Add(new IdentityError { Code = "StartWithDigit", Description = "Kullanıcı adı rakamla başlayamaz" });
                    break;
                }
            }
            
            if(errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
