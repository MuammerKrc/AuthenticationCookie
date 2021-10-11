using IdentityWithCookies.Helper;
using IdentityWithCookies.Identity;
using IdentityWithCookies.Models;
using IdentityWithCookies.VİewsModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWithCookies.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private HelperMethods _helperMethods;

        public HomeController(ILogger<HomeController> logger,UserManager<User> userManager, SignInManager<User> signInManager,HelperMethods helperMethods)
        {
            _helperMethods = helperMethods;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
            { return RedirectToAction("Index", "Member"); }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region ResetPassword
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "lütfen geçerli bir mail giriniz");
                return View(viewModel);
            }
            User user = await FindUserByEmail(viewModel.Email);
            if(User==null)
            {
                ModelState.AddModelError("", "lütfen geçerli bir mail giriniz");
                return View(viewModel);
            }
            var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = Url.Action("ResetPasswordConfirm", "home", new
            {
                userId = user.Id,
                token = resetPasswordToken,
            });
            string mesajBody = $"Parolanızı Yenilemek için lütfen <a href='https://localhost:5001{url}'>tıklayınız.</a>";
            HelperMethods.SendEmailToResetPasswordToken(viewModel.Email, mesajBody);

            return RedirectToAction("ResetPasswordViewModel", "home");
        }
        public IActionResult ResetPasswordViewModel()
        {
            return View();
        }


        public IActionResult ResetPasswordConfirm(string userId,string token)
        {
            if(userId==null || token==null)
            {
                return RedirectToAction("Index", "Home");
            }
            
            return View(new ResetPasswordConfirmModel()
            {
                Token = token
            });
        }
        [HttpPost]
        public async Task<IActionResult> ResetPasswordConfirm(ResetPasswordConfirmModel confirmModel)
        {
            if(!ModelState.IsValid)
            {
                return View(confirmModel);
            }
            var user = FindUserByEmail(confirmModel.Email).Result;
            if(user==null)
            {
                return View(confirmModel);
            }

            var result = await _userManager.ResetPasswordAsync(user, confirmModel.Token, confirmModel.Password);
            if(!result.Succeeded)
            {
                result.Errors.ToList().ForEach(x =>
                {
                    ModelState.AddModelError("", x.Description);
                });
                return View(confirmModel);
            }
            await _userManager.UpdateSecurityStampAsync(user);
            return RedirectToAction("login","home");
        }
        #endregion
        #region login
        public IActionResult Login(string returnUrl)
        {
            if(!String.IsNullOrEmpty(returnUrl))
            {
                TempData["ReturnUrl"] = returnUrl;
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginView)
        {
            if(!ModelState.IsValid)
            {
                return View(loginView);
            }
            User user = await FindUserByEmail(loginView.Email);
            if (user==null)
            {
                ModelState.AddModelError("", "Kullanıcı veya Şifreniz Hatalı");
                return View(loginView);
            }
            await _signInManager.SignOutAsync();
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, loginView.Password,loginView.RememberMe, false);

            if(result.Succeeded)
            {
                if(TempData["ReturnUrl"]!=null)
                {
                    return Redirect(TempData["ReturnUrl"].ToString());
                }
                return RedirectToAction("Index", "member");
            }
            ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı");
            
            return View(loginView);
        }


        #endregion
        #region SignUp
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            var newUSer = MakeNewUser(user);
            var result = await CreateNewUSer(newUSer, user.Password);

            if (!result.Succeeded)
            {
                result.Errors.ToList().ForEach(x =>
                {
                    ModelState.AddModelError("", x.Description);
                });
                return View(user);
            }
            return RedirectToAction("Index");
        }
        #endregion
        public User MakeNewUser(UserViewModel userModel)
        {
            return new User
            {
                Email = userModel.Email,
                PhoneNumber = userModel.PhoneNumber,
                UserName = userModel.UserName
            };
        }
        public async Task<IdentityResult> CreateNewUSer(User user, string pasword)
        {
            IdentityResult result = await _userManager.CreateAsync(user, pasword);
            return result;
        }
        public async Task<User> FindUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user;
        }
    }
}
