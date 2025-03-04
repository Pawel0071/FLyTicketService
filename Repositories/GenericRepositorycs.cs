using FLyTicketService.Data;
using FLyTicketService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FLyTicketService.Repositories
{
    public class GenericRepository<T>: IGenericRepository<T> where T : class
    {
        #region Fields

        private readonly FLyTicketDbContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly ILogger<T> _logger;

        #endregion

        #region Constructors

        public GenericRepository(FLyTicketDbContext context, ILogger<T> logger)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _logger = logger;
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            _logger.LogInformation($"Getting all entities of type {typeof(T).Name}");
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation($"Getting entity of type {typeof(T).Name} with id {id}");
            return await _dbSet.FindAsync(id);
        }

        public async Task<bool> AddAsync(T entity)
        {
            _logger.LogInformation($"Adding entity of type {typeof(T).Name}");
            await _dbSet.AddAsync(entity);
            bool result = await _context.SaveChangesAsync() > 0;
            _logger.LogInformation($"Entity of type {typeof(T).Name}{(result ? " not " : " ")}added");
            return result;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _logger.LogInformation($"Updating entity of type {typeof(T).Name}");
            _dbSet.Update(entity);
            bool result = await _context.SaveChangesAsync() > 0;
            _logger.LogInformation($"Entity of type {typeof(T).Name}{(result ? " not " : " ")}updated" );
            return result;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            _logger.LogInformation($"Removing entity of type {typeof(T).Name} with id {id}");
            T? entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                bool result = await _context.SaveChangesAsync() > 0;
                _logger.LogInformation($"Entity of type {typeof(T).Name} with id {id} removed");
                return result;
            }
            else
            {
                _logger.LogWarning($"Entity of type {typeof(T).Name} with id {id} not found");
                return false;
            }
        }
        public async Task<T?> GetByAsync(Func<T, bool> predicate)
        {
            _logger.LogInformation($"Getting entity of type {typeof(T).Name} by predicate");
            return await Task.Run(() => _dbSet.SingleOrDefault(predicate));
        }

        public async Task<IEnumerable<T>> FilterByAsync(Func<T, bool> predicate)
        {
            _logger.LogInformation($"Filtering entities of type {typeof(T).Name} by predicate");
            return await Task.Run(() => _dbSet.Where(predicate).ToList());
        }

        #endregion
        }
    }