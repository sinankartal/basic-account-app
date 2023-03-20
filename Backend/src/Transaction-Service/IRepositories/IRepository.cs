public interface IRepository<T>
{
    public Task<Guid> AddAsync(T entity);
    Task SaveAsync();
    
}