using FLyTicketService.Data;
using FLyTicketService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

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

            IQueryable<T> query = _dbSet;

            // Pobierz właściwości nawigacyjne za pomocą refleksji
            IEnumerable<string> navigations = _context.Model.FindEntityType(typeof(T))
                                                      .GetNavigations()
                                                      .Select(n => n.Name);

            // Dodaj Include dla każdej właściwości nawigacyjnej
            foreach (string navigation in navigations)
            {
                query = query.Include(navigation);
            }

            return await query.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation($"Getting entity of type {typeof(T).Name} with id {id}");

            string? keyName = _context.Model.FindEntityType(typeof(T))
                                     ?.FindPrimaryKey()
                                     ?.Properties
                                     ?.FirstOrDefault()
                                     ?.Name;

            if (keyName == null)
            {
                throw new InvalidOperationException($"Type {typeof(T).Name} has not primary key.");
            }

            IQueryable<T> query = _dbSet;

            // Pobierz właściwości nawigacyjne za pomocą refleksji
            IEnumerable<string> navigations = _context.Model.FindEntityType(typeof(T))
                                                      .GetNavigations()
                                                      .Select(n => n.Name);

            // Dodaj Include dla każdej właściwości nawigacyjnej
            foreach (string navigation in navigations)
            {
                query = query.Include(navigation);
            }

            return await query.SingleOrDefaultAsync(e => EF.Property<Guid>(e, keyName) == id);
        }

        public async Task<bool> AddAsync(T entity)
        {
            _logger.LogInformation($"Adding or updating entity of type {typeof(T).Name}");

            try
            {
                // Dodanie lub aktualizacja głównej encji
                EntityEntry<T> entry = _context.Entry(entity);

                if (entry.State == EntityState.Detached) // Jeśli encja jest odłączona, dodajemy ją do kontekstu
                {
                    await _dbSet.AddAsync(entity);
                }
                else if (entry.State == EntityState.Modified || entry.State == EntityState.Unchanged) // Obsługa aktualizacji
                {
                    _dbSet.Update(entity);
                }

                // Pobierz właściwości nawigacyjne
                IEnumerable<INavigation> navigations = _context.Model.FindEntityType(typeof(T))
                                                               .GetNavigations();

                foreach (INavigation navigation in navigations)
                {
                    NavigationEntry navigationEntry = _context.Entry(entity).Navigation(navigation.Name);

                    if (navigation.IsCollection) // Sprawdzenie, czy to kolekcja
                    {
                        CollectionEntry collection = _context.Entry(entity).Collection(navigation.Name);

                        if (collection != null && !collection.IsLoaded) // Załaduj kolekcję tylko, jeśli nie jest załadowana
                        {
                            _logger.LogInformation($"Loading collection {navigation.Name} for entity {typeof(T).Name}");
                            await collection.LoadAsync();
                        }

                        // Iteracja przez elementy kolekcji i ich dodanie lub aktualizacja
                        foreach (object? childEntity in collection.CurrentValue)
                        {
                            EntityEntry childEntry = _context.Entry(childEntity);
                            if (childEntry.State == EntityState.Detached)
                            {
                                _context.Add(childEntity);
                            }
                            else if (childEntry.State == EntityState.Modified || childEntry.State == EntityState.Unchanged)
                            {
                                _context.Update(childEntity);
                            }
                        }
                    }
                    else // Obsługa obiektu referencyjnego
                    {
                        ReferenceEntry reference = _context.Entry(entity).Reference(navigation.Name);

                        if (reference != null && reference.CurrentValue != null) // Załaduj referencję tylko, jeśli jest obecna
                        {
                            object? relatedEntity = reference.CurrentValue;
                            EntityEntry relatedEntry = _context.Entry(relatedEntity);

                            if (relatedEntry.State == EntityState.Detached)
                            {
                                _context.Add(relatedEntity);
                            }
                            else if (relatedEntry.State == EntityState.Modified || relatedEntry.State == EntityState.Unchanged)
                            {
                                _context.Update(relatedEntity);
                            }
                        }
                    }
                }

                // Zapisanie zmian
                bool result = await _context.SaveChangesAsync() > 0;

                _logger.LogInformation($"Entity of type {typeof(T).Name}{(result ? " " : " not ")}added or updated");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding or updating entity of type {typeof(T).Name}: {ex.Message}", ex);
                return false;
            }
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _logger.LogInformation($"Updating entity of type {typeof(T).Name}");

            // Update the entity
            _dbSet.Update(entity);

            // Update nested collections
            IEnumerable<string> navigations = _context.Model.FindEntityType(typeof(T))
                                                      .GetNavigations()
                                                      .Where(n => n.IsCollection)
                                                      .Select(n => n.Name);

            foreach (string navigation in navigations)
            {
                CollectionEntry collection = _context.Entry(entity).Collection(navigation);
                if (collection != null)
                {
                    await collection.LoadAsync();
                }
            }

            bool result = await _context.SaveChangesAsync() > 0;
            _logger.LogInformation($"Entity of type {typeof(T).Name}{(result ? " not " : " ")}updated");
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

            _logger.LogWarning($"Entity of type {typeof(T).Name} with id {id} not found");
            return false;
        }

        public async Task<T?> GetByAsync(Func<T, bool> predicate)
        {
            _logger.LogInformation($"Getting entity of type {typeof(T).Name} by predicate");

            IQueryable<T> query = _dbSet;

            // Pobierz właściwości nawigacyjne za pomocą refleksji
            IEnumerable<INavigation> navigations = _context.Model.FindEntityType(typeof(T))
                                                      .GetNavigations();

            // Dodaj Include dla każdej właściwości nawigacyjnej
            foreach (INavigation navigation in navigations)
            {
                query = query.Include(navigation.Name);

                // Pobierz właściwości nawigacyjne dla zagnieżdżonych obiektów
                IEnumerable<INavigation> nestedNavigations = navigation.TargetEntityType.GetNavigations();
                foreach (INavigation nestedNavigation in nestedNavigations)
                {
                    query = query.Include($"{navigation.Name}.{nestedNavigation.Name}");
                }
            }

            return await Task.Run(() => query.SingleOrDefault(predicate));
        }

        public async Task<IEnumerable<T>> FilterByAsync(Func<T, bool> predicate)
        {
            _logger.LogInformation($"Filtering entities of type {typeof(T).Name} by predicate");

            IQueryable<T> query = _dbSet;

            // Pobierz właściwości nawigacyjne za pomocą refleksji
            IEnumerable<string> navigations = _context.Model.FindEntityType(typeof(T))
                                                      .GetNavigations()
                                                      .Select(n => n.Name);

            // Dodaj Include dla każdej właściwości nawigacyjnej
            foreach (string navigation in navigations)
            {
                query = query.Include(navigation);
            }

            List<T> a = await Task.Run(() => query.Where(predicate).ToList());

            return a;
        }

        #endregion
    }
}