using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.IRepositories;
using Persistence.Models;

namespace Persistence;

public abstract class Repository<T> : IRepository<T> where T : BaseEntity
{
    #region property

    private readonly AccountDbContext _accountDbContext;
    private DbSet<T> entities;

    #endregion

    #region Constructor

    protected Repository(AccountDbContext accountDbContext)
    {
        _accountDbContext = accountDbContext;
        entities = _accountDbContext.Set<T>();
    }

    #endregion

    public virtual async Task<T> FindAsync(Guid id)
    {
        return await entities.FindAsync(id);
    }
    
    public virtual async Task<bool> IsExist(Guid id)
    {
        return await entities.AnyAsync(e=>e.Id.Equals(id));
    }

    public virtual async Task SaveAsync()
    {
        await _accountDbContext.SaveChangesAsync();
    }

    public virtual async Task<Guid> AddAsync(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException("entity");
        }

        entity.Id = Guid.NewGuid();
        entity.CreateDate = DateTime.Now;
        entity.ModifyDate = DateTime.Now;
        await entities.AddAsync(entity);
        return entity.Id;
    }

    public virtual void Update(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException("entity");
        }
        entity.ModifyDate = DateTime.Now;
        entities.Update(entity);
        _accountDbContext.SaveChanges();
    }
}