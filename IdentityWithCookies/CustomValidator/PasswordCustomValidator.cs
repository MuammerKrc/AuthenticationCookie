using IdentityWithCookies.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWithCookies.CustomValidator
{
    public class PasswordCustomValidator : IPasswordValidator<User>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
        {
            List<IdentityError> errors = new List<IdentityError>();

            if (password.ToLower().Contains(user.UserName))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordContainsUsername",
                    Description = "şifre kullanıcı adı içeremez"
                }) ;
            }

            //if(password.ToLower().Contains("1234"))
            //{
            //    errors.Add(new IdentityError { Code = "PasswordContains1234", Description = "Şifreniz ardışık sayı içeremez" });
            //}

            if(errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
