using BooksManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksManagementSystem.DAL.Borrowing
{
    public interface IBorrowingDataRepository
    {
        Task Create(BorrowingViewModel author);
        Task<BorrowingViewModel> GetDetails(int id);

        DbSet<BorrowingViewModel> GetAll();

        Task<BorrowingViewModel?> GetNotTracking(int id);

        Task Edit(BorrowingViewModel author);

        Task Delete(int id);

        Task<IQueryable<BorrowingViewModel>> GetAllBorrowingFullInfo(bool isAdmin, string userName);
    }
}