using BooksManagementSystem.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace BooksManagementSystem.DAL.Accounts
{
    public class AccountsRepository : IAccountsRepository
    {
        private readonly UserManager<BooksManagementSystemUser> _userManager;
        private readonly SignInManager<BooksManagementSystemUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountsRepository(UserManager<BooksManagementSystemUser> userManager,
            SignInManager<BooksManagementSystemUser> signInManager,
            RoleManager<IdentityRole> rolemMnager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = rolemMnager;
            //Task.Run(() => this.CreateRoles()).Wait();
        }

        #region Temp
        //private async Task CreateRoles()
        //{
        //    //initializing custom roles 

        //    string[] roleNames = { "Admin", "User" };
        //    IdentityResult roleResult;

        //    foreach (var roleName in roleNames)
        //    {
        //        var roleExist = await _roleManager.RoleExistsAsync(roleName);
        //        // ensure that the role does not exist
        //        if (!roleExist)
        //        {
        //            //create the roles and seed them to the database: 
        //            roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
        //        }
        //    }

        //    // find the user with the admin email 
        //    var user = await _userManager.FindByEmailAsync("admin.admin@gmail.com");

        //    // check if the user exists
        //    if (user == null)
        //    {
        //        //Here you could create the super admin who will maintain the web app
        //        var poweruser = new BooksManagementSystemUser
        //        {
        //            UserName = "Admin",
        //            Email = "admin@email.com",
        //        };
        //        string adminPassword = "p@$$w0rd";

        //        var createPowerUser = await _userManager.CreateAsync(poweruser, adminPassword);
        //        if (createPowerUser.Succeeded)
        //        {
        //            //here we tie the new user to the role
        //            await _userManager.AddToRoleAsync(poweruser, "Admin");
        //        }
        //    }
        //    else
        //    {
        //        await _userManager.AddToRoleAsync(user, "Admin");
        //    }
        //}
        #endregion

        public async Task<IdentityResult> CreateUserAsync(BooksManagementSystemUser user, string password)
        {
            var newIdentityUser = await _userManager.CreateAsync(user, password);
            if (newIdentityUser != null && !newIdentityUser.Succeeded)
            {
                return newIdentityUser;
            }
            return await _userManager.AddToRoleAsync(user, "User");
        }

        public async Task<SignInResult> PasswordSignInAsync(string userEmail, string password, bool isPersistent, bool lockoutOnFailure = false)
        {
            return await _signInManager.PasswordSignInAsync(userEmail,
                      password, isPersistent, lockoutOnFailure);
        }

        public async Task UserSignInAsync(BooksManagementSystemUser user, bool isPersistent = false)
        {
            await _signInManager.SignInAsync(user, isPersistent: isPersistent);
        }

        public async Task UserSignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
