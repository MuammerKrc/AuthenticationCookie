using IdentityWithCookies.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWithCookies.Controllers
{
    public class BaseController : Controller
    {
        protected readonly UserManager<User> _userManager;
        protected readonly SignInManager<User> _signInManager;
        protected readonly RoleManager<Role> _roleManager;
        public BaseController(UserManager<User> userManager, SignInManager<User> signInManager,RoleManager<Role> roleManager=null)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        protected User CurrentUser() => _userManager.FindByNameAsync(User.Identity.Name).Result;

        protected void AddModelError(IdentityResult result)
        {
            if(result.Errors.Any())
            {
                result.Errors.ToList().ForEach(x => ModelState.AddModelError("", x.Description));
            }
        }
         
    }
}
