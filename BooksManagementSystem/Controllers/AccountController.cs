using BooksManagementSystem.Areas.Identity.Data;
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


        public AccountController(UserManager<BooksManagementSystemUser> userManager,
            SignInManager<BooksManagementSystemUser> signInManager,
            RoleManager<IdentityRole> rolemMnager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = rolemMnager;

            //Task.Run(() => this.CreateRoles()).Wait();

        }

        private async Task CreateRoles()
        {
            //initializing custom roles 
           
            string[] roleNames = { "Admin", "User" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                // ensure that the role does not exist
                if (!roleExist)
                {
                    //create the roles and seed them to the database: 
                    roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // find the user with the admin email 
            var user = await _userManager.FindByEmailAsync("admin.admin@gmail.com");

            // check if the user exists
            if (user == null)
            {
                //Here you could create the super admin who will maintain the web app
                var poweruser = new BooksManagementSystemUser
                {
                    UserName = "Admin",
                    Email = "admin@email.com",
                };
                string adminPassword = "p@$$w0rd";

                var createPowerUser = await _userManager.CreateAsync(poweruser, adminPassword);
                if (createPowerUser.Succeeded)
                {
                    //here we tie the new user to the role
                    await _userManager.AddToRoleAsync(poweruser, "Admin");
                }
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
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
                var result = await _userManager.CreateAsync(user, model.Password);

                // If user is successfully created, sign-in the user using
                // SignInManager and redirect to index action of HomeController
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
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
            await _signInManager.SignOutAsync();
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
                var result = await _signInManager.PasswordSignInAsync(model.Email,
                    model.Password, model.RememberMe, false);

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
