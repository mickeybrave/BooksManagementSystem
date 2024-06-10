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
            if (await _context.SaveChangesAsync() > 0)
            {
                var borrowedBook = await _context.Books.FindAsync(borrowing.BookId);
                if (borrowedBook != null)
                {
                    borrowedBook.IsAvailable = borrowing.ReturnedDate != null;//book is borrowed already
                    _context.Update(borrowedBook);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task Delete(int id)
        {
            var borrowingToDelete = new BorrowingViewModel() { Id = id };
            _context.Entry(borrowingToDelete).State = EntityState.Deleted;
            if (await _context.SaveChangesAsync() > 0)
            {
                var borrowedBook = await _context.Books.FindAsync(borrowingToDelete.BookId);
                if (borrowedBook != null)
                {
                    borrowedBook.IsAvailable = true;//when borrowing is removed by admin, it is equivalent to returning the book
                    _context.Update(borrowedBook);
                    await _context.SaveChangesAsync();
                }
            }
        }
        public async Task Edit(BorrowingViewModel borrowing)
        {
            _context.Update(borrowing);
            if (await _context.SaveChangesAsync() > 0)
            {
                var borrowedBook = await _context.Books.FindAsync(borrowing.BookId);
                if (borrowedBook != null)
                {
                    borrowedBook.IsAvailable = borrowing.ReturnedDate != null;//book is returned
                    _context.Update(borrowedBook);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public DbSet<BorrowingViewModel> GetAll()
        {
            return _context.Borrowings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isAdmin"></param>
        /// <param name="userName">is a unique username that can be only 1 in the system. userName == email address</param>
        /// <returns></returns>
        public async Task<IQueryable<BorrowingViewModel>> GetAllBorrowingFullInfo(bool isAdmin, string userName)
        {

            return await Task.Run(() => (from b in _context.Borrowings
                                         join book in _context.Books on b.BookId equals book.Id
                                         join user in _context.Users on b.UserId equals user.Id
                                         join author in _context.Authors on book.AuthorId equals author.Id
                                         where (user.UserName == userName || isAdmin == true)
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
                                         }));

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
