namespace Persistence.IRepositories;

public interface IRepository<T>
{
    public Task<Guid> AddAsync(T entity);
    
    Task<T> FindAsync(Guid id);

    Task<bool> IsExist(Guid id);

    Task SaveAsync();

    void Update(T item);
}