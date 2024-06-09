using BooksManagementSystem.Data;
using BooksManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksManagementSystem.DAL.Borrowing
{
    public class BorrowingDataRepository : IBorrowingDataRepository
    {
        private readonly BooksManagementSystemContext _context;

        public BorrowingDataRepository(BooksManagementSystemContext context)
        {
            _context = context;
        }

        public async Task Create(BorrowingViewModel borrowing)
        {
            _context.Add(borrowing);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var borrowingToDelete = new BorrowingViewModel() { Id = id };
            _context.Entry(borrowingToDelete).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public async Task Edit(BorrowingViewModel borrowing)
        {
            _context.Update(borrowing);
            await _context.SaveChangesAsync();
        }

        public DbSet<BorrowingViewModel> GetAll()
        {
            return _context.Borrowings;
        }

        public IQueryable<BorrowingViewModel> GetAllBorrowingFullInfo()
        {

            var borrowings = (from b in _context.Borrowings
                              join book in _context.Books on b.BookId equals book.Id
                              join user in _context.Users on b.UserId equals user.Id
                              join author in _context.Authors on book.AuthorId equals author.Id
                              select new BorrowingViewModel
                              {
                                  BorrowedDate = b.BorrowedDate,
                                  BookViewModel = new BookViewModel
                                  {
                                      Title = book.Title,
                                      AuthorViewModel = author,
                                      AuthorId = book.AuthorId,
                                      Category = book.Category,
                                      Description = book.Description,
                                      IsAvailable = book.IsAvailable,
                                      Id = book.Id,
                                      ISBN = book.ISBN
                                  },
                                  BooksManagementSystemUser = user,
                                  BookId = book.Id,
                                  Id = b.Id,
                                  ReturnedDate = b.ReturnedDate,
                                  UserId = b.UserId
                              });

            return borrowings;
        }

        public async Task<BorrowingViewModel> GetDetails(int id)
        {
            return await _context.Borrowings.FindAsync(id);
        }

        public async Task<BorrowingViewModel?> GetNotTracking(int id)
        {
            return await _context.Borrowings
                 .AsNoTracking()// EF extension to update existing user instead of inserting a new one
                 .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
