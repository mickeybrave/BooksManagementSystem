using BooksManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksManagementSystem.DAL.Books
{
    public interface IBooksDataRepository
    {
        Task Create(BookViewModel author);
        Task<BookViewModel> GetDetails(int id);

        IOrderedQueryable<BookViewModel> GetAll();

        Task<BookViewModel?> GetNotTracking(int id);

        Task Edit(BookViewModel author);

        Task Delete(int id);

        IQueryable<BookViewModel> GetAllBooksFullInfo();

        Task<IQueryable<BookViewModel>> GetAllAvailableBooks(int? bookId);
    }
}
