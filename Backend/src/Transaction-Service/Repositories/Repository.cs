using TransactionService.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence;

public abstract class Repository<T> : IRepository<T> where T : Transaction
{
    #region property

    private readonly TransactionDbContext _context;
    private DbSet<T> entities;

    #endregion

    #region Constructor

    protected Repository(TransactionDbContext context)
    {
        _context = context;
        entities = _context.Set<T>();
    }

    #endregion
    

    public virtual async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public virtual async Task<Guid> AddAsync(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException("entity");
        }

        entity.Id = Guid.NewGuid();
        entity.Date = DateTime.Now;
        await entities.AddAsync(entity);
        await SaveAsync();
        return entity.Id;
    }
}