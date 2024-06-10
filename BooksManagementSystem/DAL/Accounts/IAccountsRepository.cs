using BooksManagementSystem.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace BooksManagementSystem.DAL.Accounts
{
    public interface IAccountsRepository
    {
        Task<IdentityResult> CreateUserAsync(BooksManagementSystemUser user, string password);

        Task UserSignInAsync(BooksManagementSystemUser user, bool isPersistent = false);

        Task UserSignOutAsync();

        Task<SignInResult> PasswordSignInAsync(string userEmail, string password,
            bool isPersistent, bool lockoutOnFailure = false);

        Task<List<BooksManagementSystemUser>> GetUsersAsync();

        Task<BooksManagementSystemUser> GetUsersAsync(string userId);

        Task<BooksManagementSystemUser> GetUsersByEmailAsync(string userEmail);
    }
}
