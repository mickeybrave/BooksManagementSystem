using BooksManagementSystem.Areas.Identity.Data;
using BooksManagementSystem.DAL.Accounts;
using BooksManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BooksManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<BooksManagementSystemUser> _userManager;
        private readonly SignInManager<BooksManagementSystemUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAccountsRepository _accountsRepository;

        public AccountController(UserManager<BooksManagementSystemUser> userManager,
            SignInManager<BooksManagementSystemUser> signInManager,
            RoleManager<IdentityRole> rolemMnager, IAccountsRepository accountsRepository)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = rolemMnager;
            this._accountsRepository = accountsRepository;



        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Copy data from RegisterViewModel to IdentityUser
                var user = new BooksManagementSystemUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Firstname = model.Firstname,
                    LastName = model.Lastname,
                    EmailConfirmed = true// set to prevent "invalid login" validation error. it happens when EmailConfirmed in AspNetUsers is set to false. recommended configuration does not work.
                };

                // Store user data in AspNetUsers database table
                var result = await _accountsRepository.CreateUserAsync(user, model.Password);

                // If user is successfully created, sign-in the user using
                // SignInManager and redirect to index action of HomeController
                if (result.Succeeded)
                {
                    await _accountsRepository.UserSignInAsync(user);
                    return RedirectToAction("index", "home");
                }

                // If there are any errors, add them to the ModelState object
                // which will be displayed by the validation summary tag helper
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _accountsRepository.UserSignOutAsync();
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        //returnUrl should be nullable to prevent ModelState valication error when returnUrl is not set up
        public async Task<IActionResult> Login(LogInViewModel model, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountsRepository.PasswordSignInAsync(model.Email,
                    model.Password, model.RememberMe);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("index", "home");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }

            return View(model);


        }

    }
}
