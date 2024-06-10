using BooksManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksManagementSystem.DAL.Books
{
    public interface IBooksDataRepository
    {
        Task Create(BookViewModel book);
        Task<BookViewModel> GetDetails(int id);

        IOrderedQueryable<BookViewModel> GetAll();

        Task<BookViewModel?> GetNotTracking(int id);

        Task Edit(BookViewModel book);

        Task Delete(int id);

        Task<IQueryable<BookViewModel>> GetAllBooksFullInfo();

        Task<IQueryable<BookViewModel>> GetAllAvailableBooks(int? bookId);
    }
}
