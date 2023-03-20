using Persistence.Models;

namespace Persistence.IRepositories;

public interface IUserRepository : IRepository<User>
{
    Task<List<User>> GetAll();
}