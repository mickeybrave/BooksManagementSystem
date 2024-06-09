using BooksManagementSystem.DAL.Authors;
using BooksManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksManagementSystem.DAL.GenericRepository
{
    public interface IEntity
    {
        int Id { get; set; }
    }
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly DbContext _context;

        public GenericRepository(DbContext context)
        {
            _context = context;
        }

        public async Task Create(TEntity entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entityToDelete = await _context.FindAsync<TEntity>(id);
            if (entityToDelete != null)
            {
                _context.Remove(entityToDelete);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Edit(TEntity entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }

        public async Task<TEntity?> GetDetails(int id)
        {
            return await _context.FindAsync<TEntity>(id);
        }

        public async Task<TEntity?> GetNotTracking(int id)
        {
            return await _context.Set<TEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
