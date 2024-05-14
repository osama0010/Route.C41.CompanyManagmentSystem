using CompanyManagmentSystem.Controllers;
using CompanyManagmentSystem.DAL.Models;
using CompanyManagmentSystem.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CompanyManagmentSystem.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
			_userManager = userManager;
			_signInManager = signInManager;
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
            if(ModelState.IsValid)
            {
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
            }
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
            if (ModelState.IsValid)
            {
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
            }
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
    }
}
