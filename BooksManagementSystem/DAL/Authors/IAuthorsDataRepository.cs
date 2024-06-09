using BooksManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksManagementSystem.DAL.Authors
{
    public interface IAuthorsDataRepository
    {
        Task Create(AuthorViewModel author);
        Task<AuthorViewModel> GetDetails(int id);

        DbSet<AuthorViewModel> GetAll();

        Task<AuthorViewModel?> GetNotTracking(int id);

        Task Edit(AuthorViewModel author);

        Task Delete(int id);
    }
}
