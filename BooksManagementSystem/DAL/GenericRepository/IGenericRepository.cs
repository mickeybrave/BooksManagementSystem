namespace BooksManagementSystem.DAL.GenericRepository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task Create(TEntity author);
        Task<TEntity> GetDetails(int id);

        IQueryable<TEntity> GetAll();

        Task<TEntity?> GetNotTracking(int id);

        Task Edit(TEntity author);

        Task Delete(int id);
    }
}
