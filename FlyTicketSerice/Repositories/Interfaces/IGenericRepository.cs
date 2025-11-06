namespace FLyTicketService.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<T?> GetByAsync(Func<T, bool> predicate);
        Task<bool> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> RemoveAsync(Guid id);
        Task<IEnumerable<T>> FilterByAsync(Func<T, bool> predicate);
    }
}
