using BooksManagementSystem.Data;
using BooksManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksManagementSystem.DAL.Books
{
    public class BooksDataRepository : IBooksDataRepository
    {
        private readonly BooksManagementSystemContext _context;

        public BooksDataRepository(BooksManagementSystemContext context)
        {
            _context = context;
        }

        public async Task Create(BookViewModel book)
        {
            _context.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var authorToDelete = new BookViewModel() { Id = id };
            _context.Entry(authorToDelete).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public async Task Edit(BookViewModel book)
        {
            _context.Update(book);
            if (await _context.SaveChangesAsync() > 0)
            {
                foreach (var borrowing in _context.Borrowings.Where(borrowing => borrowing.BookId == book.Id))
                {
                    borrowing.ReturnedDate = DateTime.Now;
                    _context.Update(borrowing);
                }
                await _context.SaveChangesAsync();
            }
        }

        public IOrderedQueryable<BookViewModel> GetAll()
        {
            return _context.Books.OrderBy(x => x.Title);
        }

        public async Task<BookViewModel> GetDetails(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<BookViewModel?> GetNotTracking(int id)
        {
            return await _context.Books
                 .AsNoTracking()// EF extension to update existing user instead of inserting a new one
                 .FirstOrDefaultAsync(m => m.Id == id);
        }

        public IQueryable<BookViewModel> GetAllBooksFullInfo()
        {

            return (from book in _context.Books
                    join author in _context.Authors on book.AuthorId equals author.Id
                    orderby book.Title
                    select new BookViewModel
                    {
                        Title = book.Title,
                        AuthorViewModel = author,
                        AuthorId = book.AuthorId,
                        Category = book.Category,
                        Description = book.Description,
                        IsAvailable = book.IsAvailable,
                        Id = book.Id,
                        ISBN = book.ISBN
                    });
        }


        public async Task<IQueryable<BookViewModel>> GetAllAvailableBooks(int? bookId)
        {
            return await Task.Run(() => (from book in _context.Books
                                         where book.IsAvailable || bookId.GetValueOrDefault() == book.Id
                                         select book));
        }
    }
}
