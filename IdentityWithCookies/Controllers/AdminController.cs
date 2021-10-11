using IdentityWithCookies.Identity;
using IdentityWithCookies.VİewsModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;

namespace IdentityWithCookies.Controllers
{
    public class AdminController : BaseController
    {
        
        public AdminController(UserManager<User> userManager, RoleManager<Role> roleManager) : base(userManager, null, roleManager)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Claims()
        {
            return View(User.Claims.ToList());
        }

        public IActionResult Users()
        {
            return View(_userManager.Users);
        }
        public IActionResult RoleCreate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RoleCreate(RoleViewModel roleViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(roleViewModel);
            }
            Role role = new Role { Name = roleViewModel.Name };
            IdentityResult result = _roleManager.CreateAsync(role).Result;
            if (!result.Succeeded)
            {
                AddModelError(result);
                return View(roleViewModel);
            }

            return RedirectToAction("roles");
        }
        public IActionResult Roles()
        {

            return View(_roleManager.Roles.ToList().Adapt<List<RoleViewModel>>());
        }
        public async Task<IActionResult> RoleDelete(string roleId)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Role bulunamadı");
                return RedirectToAction("Roles");
            }
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ModelState.AddModelError("", "Role bulunamadı");
                return RedirectToAction("Roles");
            }

            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Role bulunamadı");
                AddModelError(result);
                return RedirectToAction("Roles");
            }

            return RedirectToAction("Roles");

        }
        public IActionResult RoleUpdate(string roleId)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Role bulunamadı");
                return RedirectToAction("Roles");
            }
            var role = _roleManager.FindByIdAsync(roleId).Result;
            if (role == null)
            {
                ModelState.AddModelError("", "Role bulunamadı");
                return RedirectToAction("Roles");
            }
            return View(role.Adapt<RoleViewModel>());
        }
        [HttpPost]
        public async Task<IActionResult> RoleUpdate(RoleViewModel roleView)
        {
            if (!ModelState.IsValid)
            {
                return View(roleView);
            }
            var role = await _roleManager.FindByIdAsync(roleView.Id);
            if (role != null)
            {
                role.Name = roleView.Name;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                AddModelError(result);
            }
            return RedirectToAction("Roles");

        }
        public async Task<IActionResult> RoleAssign(string Id)
        {
            if (ModelState.IsValid)
            {

                var user = _userManager.FindByIdAsync(Id).Result;
                if (user != null)
                {
                    ViewData["userName"] = user.UserName;
                }
                List<Role> AllRole = _roleManager.Roles.ToList();
                List<string> userHaveRoleString = await _userManager.GetRolesAsync(user) as List<string>;
                List<Role> UserHave = new List<Role>();
                userHaveRoleString.ForEach(i => UserHave.Add( _roleManager.FindByNameAsync(i).Result));
                if(UserHave==null)
                {
                    UserHave = new List<Role>();
                }
                
                List<Role> UserDontHave = new List<Role>();
                RoleList Roles = new RoleList();
                UserDontHave = AllRole.Where(i => !(UserHave.Any(y => y.Name.Contains(i.Name)))).ToList();
                Roles.OwnerRole = UserHave.Adapt<List<RoleEditViewModel>>();
                Roles.AddRole = UserDontHave.Adapt<List<RoleEditViewModel>>();
                Roles.DeleteRole = UserHave.Adapt<List<RoleEditViewModel>>();
                Roles.UserId = user.Id;
                return View(Roles);
            }
            ModelState.AddModelError("", "Bir hata oluştu");
            return RedirectToAction("Users");
        }
        [HttpPost]
        public async Task<IActionResult> RoleAssign(RoleList Roles)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(Roles.UserId);
                
                if(Roles.AddRole.Any(i=>i.Exist))
                {
                    foreach (var item in Roles.AddRole.Where(i=>i.Exist))
                    {
                        IdentityResult result=await _userManager.AddToRoleAsync(user, item.Name);
                        if(!result.Succeeded)
                        {
                            AddModelError(result);
                            return View(Roles);
                        }
                    }
                }
                if(Roles.DeleteRole.Any(i=>i.Exist))
                {
                    foreach(var item in Roles.DeleteRole.Where(i=>i.Exist))
                    {
                        IdentityResult result = await _userManager.RemoveFromRoleAsync(user, item.Name);
                        if(!result.Succeeded)
                        {
                            AddModelError(result);
                            return View(Roles);
                        }
                    }
                }
                return RedirectToAction("RoleAssign", "admin",new { Id=Roles.UserId});
            }
            return View(Roles);
        }
    }
}
