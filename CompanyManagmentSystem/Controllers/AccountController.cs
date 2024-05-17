﻿using CompanyManagmentSystem.Controllers;
using CompanyManagmentSystem.DAL.Models;
using CompanyManagmentSystem.PL.Services.EmailSender;
using CompanyManagmentSystem.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using static System.Collections.Specialized.BitVector32;
using System.Xml.Linq;

namespace CompanyManagmentSystem.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IEmailSender _emailSender;
		private readonly IConfiguration _configuration;

		public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            IConfiguration configuration)
        {
			_userManager = userManager;
			_signInManager = signInManager;
			_emailSender = emailSender;
			_configuration = configuration;
		}

        #region Sign Up
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if(!ModelState.IsValid) return View(model);

			var user = await _userManager.FindByNameAsync(model.UserName);

                if(user is null)
                {
					user = new ApplicationUser
					{
						FName = model.FirstName,
						LName = model.LastName,
						UserName = model.UserName,
						Email = model.Email,
						IsAgree = model.IsAgree
					};

                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                        return RedirectToAction(nameof(SignIn));

                    foreach (var Error in result.Errors)
                        ModelState.AddModelError(string.Empty, Error.Description);
                }

                ModelState.AddModelError(string.Empty, "This Username is Already Used");
            return View(model);
        }
        #endregion

        #region Sign In

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

				var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if(flag)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.rememberMe, false);

                        if (result.IsLockedOut)
							ModelState.AddModelError(string.Empty, "Your Account is Locked");

                        if (result.Succeeded)
                            return RedirectToAction(nameof(HomeController.Index), "Home");

                        if (result.IsNotAllowed)
							ModelState.AddModelError(string.Empty, "Your Email is not Confirmed Yet");
                        

					}
				}
                ModelState.AddModelError(string.Empty, "Invalid Login");
                return View(model);
        }

        #endregion

        #region Sign Out
        public async new Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }
		#endregion


		public IActionResult Forgetpassword()
		{
			return View();
		}
        [HttpPost]
        public async Task<IActionResult> Forgetpassword(ForgetPasswordViewModel model)
		{
            if(!ModelState.IsValid) return View(model);

			var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
				{
                    var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);


					var resetPasswordUrl = Url.Action("ResetPassword", "Account",
                        new { email = user.Email, token = resetPasswordToken }, "https", "localhost:44391");

					await _emailSender.SendAsync(
						_configuration["EmailSetting:SenderEmail"],
                        model.Email,
                        "Reset your Password",
                        resetPasswordUrl);

                    return  RedirectToAction(nameof(CheckYourInbox));
				}
                ModelState.AddModelError(string.Empty, "There is No Account with this Email!!");
			return View(model);

		}


		public IActionResult CheckYourInbox()
        {
            return View();
        }

        [HttpGet]
		public IActionResult ResetPassword(string email, string token)
        {
            TempData["Email"] = email;
            TempData["Token"] = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if(!ModelState.IsValid) return View(model);

				var email = TempData["Email"] as string;
                var token = TempData["Token"] as string;

				var user = await _userManager.FindByEmailAsync(email);

                if (user is not null)
                {
					await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                    return RedirectToAction(nameof(SignIn));
				}

				ModelState.AddModelError(string.Empty, "Url is Invalid");
			return View();
        }
	}
}