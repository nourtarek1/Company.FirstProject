using Company.FirstProject.DAL.Models;
using Company.FirstProject.PL.DTOS;
using Company.FirstProject.PL.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.FirstProject.PL.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        #region SignUp
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDtos model)
        {
            if (ModelState.IsValid)
            {
                var user =await _userManager.FindByNameAsync(model.UserName);
                if(user is null)
                {
                    user = await _userManager.FindByEmailAsync(model.Email);

                    if(user is null)
                    {
                        user = new AppUser()
                        {
                            UserName = model.UserName,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            IsAgree = model.IsAgree
                        };
                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("SignIn");
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                    }

                }
                ModelState.AddModelError("", "Username or Email already exists.");

            }
            return View(model);
        }
        #endregion
        #region SignIn
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInDtos model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if(user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user ,model.Password);
                    if (flag)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user,model.Password,false,false);
                        if (result.Succeeded) {
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }

                    }
                }
                ModelState.AddModelError("", "Account is locked due to multiple failed login attempts. Try again later.");
            }

            return View(model);
        }
        #endregion

        #region SignOut
        [HttpGet]
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }
        #endregion


        #region ForgetPassword
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendResetPassword(ForgetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if(user is not null)
                {
                    // Create Token
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    // Create URL
                    var url = Url.Action("ResetPassword", "Auth",
                        new { email = model.Email, token  }, Request.Scheme);
                    //Create Email
                    var email = new Email()
                    {
                        To = model.Email,
                        Subject = "Reset Password",
                        Body = $"Click here to reset your password: <a href='{url}'>Reset Password</a>"
                    };
                    //send Email
                    var flag=EmailSettings.SendEmail(email);
                    if (flag)
                    {
                        return RedirectToAction("CheckYourInbox");
                    }
                }
            }
            ModelState.AddModelError("","Invalid Reset Password");
            return View("ForgetPassword",model);
        }

        [HttpGet]
        public IActionResult CheckYourInbox()
        {
            return View();
        }
        #endregion


        #region RsetPassword

        [HttpGet]
        public IActionResult ResetPassword(string email , string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string; 
                var token = TempData["token"] as string;

                if (email is null || token is null) return BadRequest("Invali opretion");
                var user = await _userManager.FindByEmailAsync(email);
                if(user is not null)
                {
                    var result = await _userManager.ResetPasswordAsync(user,token,model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("SignIn");
                    }
                }
                ModelState.AddModelError("", "Invalid Reset Password");

            }
            return View();
        }
        #endregion
    }
}
