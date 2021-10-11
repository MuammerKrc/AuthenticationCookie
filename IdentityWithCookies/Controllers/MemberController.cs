using IdentityWithCookies.Enums;
using IdentityWithCookies.Identity;
using IdentityWithCookies.VİewsModel;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWithCookies.Controllers
{
    [Authorize]
    public class MemberController : BaseController
    {
        
        public MemberController(UserManager<User> userManager, SignInManager<User> signManager):base(userManager,signManager)
        {
           
        }

        public  IActionResult Index()
        {
            User user = CurrentUser();
            UserViewModel viewModel = user.Adapt<UserViewModel>();
            return View(viewModel);
        }
        [Authorize(Policy ="IstanbulPolicy")]
        public IActionResult Istanbul()
        {
            return View();
        }


        [Authorize(Roles ="Editor,Admin")]
        public IActionResult Editor()
        {
            return View();
        }
        [Authorize(Roles = "Manager,Admin")]
        public IActionResult Manager()
        {
            return View();
        }
        public IActionResult UserEdit( )
        {
            var user = CurrentUser();

            UserViewModel userView = user.Adapt<UserViewModel>();
            ViewData["type"]= new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Selected=false,Text=Gender.Belirtilmemiş.ToString(),Value=((int)Gender.Belirtilmemiş).ToString()},
                new SelectListItem { Selected=false,Text=Gender.Bay.ToString(),Value=((int)Gender.Bay).ToString()},
                new SelectListItem { Selected=false,Text=Gender.Bayan.ToString(),Value=((int)Gender.Bayan).ToString()},

            });
            

            return View(userView);
        }
        [HttpPost]
        public async Task<IActionResult> UserEdit(UserViewModel userview,IFormFile userPicture)
        {
            ModelState.Remove("Password");
            if (!ModelState.IsValid)
            {
                return View(userview);

            }
            var user = CurrentUser();

            if (userPicture!=null && userPicture.Length>0)
            {
                var userPath = Guid.NewGuid().ToString() + Path.GetExtension(userPicture.FileName);
                var pathss = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\picture", userPath);

                using(var stream =new FileStream(pathss, FileMode.Create))
                {
                    await userPicture.CopyToAsync(stream);
                    user.PictureUrl =$"/picture/"+userPath;
                }
            }


            user.UserName = userview.UserName;
            user.Email = userview.Email;
            user.PhoneNumber = userview.PhoneNumber;
            user.City = userview.City;
            user.BirthDay = userview.BirthDay;
            user.Gender = ((int)userview.Gender);


            var result = await _userManager.UpdateAsync(user);
            if(!result.Succeeded)
            {
                AddModelError(result);
                return View(userview);
            }
            await _userManager.UpdateSecurityStampAsync(user);
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, true);
            return RedirectToAction("Index");
        }


        public IActionResult PasswordChange()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel passwordChangeViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(passwordChangeViewModel);
            }
            var user = CurrentUser();
            if (user == null)
            {
                return View(passwordChangeViewModel);
            }

            var result = await _userManager.ChangePasswordAsync(user, passwordChangeViewModel.PasswordOld, passwordChangeViewModel.PasswordNew);
            if (!result.Succeeded)
            {
                AddModelError(result);
                return View(passwordChangeViewModel);
            }

            await _userManager.UpdateSecurityStampAsync(user);
            return RedirectToAction("login", "Home");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        public void LogOut()
        {
            var user = CurrentUser();
            _signInManager.SignOutAsync();
        }
    }
}
