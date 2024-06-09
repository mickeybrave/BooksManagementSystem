using BooksManagementSystem.Data;
using BooksManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksManagementSystem.DAL.Authors
{


    public class AuthorstDataRepository : IAuthorsDataRepository
    {
        private readonly BooksManagementSystemContext _context;

        public AuthorstDataRepository(BooksManagementSystemContext context)
        {
            _context = context;
        }

        public async Task Create(AuthorViewModel author)
        {
            _context.Add(author);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var authorToDelete = new AuthorViewModel() { Id = id };
            _context.Entry(authorToDelete).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public async Task Edit(AuthorViewModel author)
        {
            _context.Update(author);
            await _context.SaveChangesAsync();
        }

        public DbSet<AuthorViewModel> GetAll()
        {
            return _context.Authors;
        }

        public async Task<AuthorViewModel> GetDetails(int id)
        {
            return await _context.Authors.FindAsync(id);
        }

        public async Task<AuthorViewModel?> GetNotTracking(int id)
        {
            return await _context.Authors
                 .AsNoTracking()// EF extension to update existing user instead of inserting a new one
                 .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
