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

        public async Task Create(BookViewModel author)
        {
            _context.Add(author);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var authorToDelete = new BookViewModel() { Id = id };
            _context.Entry(authorToDelete).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public async Task Edit(BookViewModel author)
        {
            _context.Update(author);
            await _context.SaveChangesAsync();
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
    }
}
