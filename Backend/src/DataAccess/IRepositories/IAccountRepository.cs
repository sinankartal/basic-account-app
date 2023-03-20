using Persistence.Models;

namespace Persistence.IRepositories;

public interface IAccountRepository: IRepository<Account>
{
    Task<List<Account>> GetUserAccounts(Guid userId);
}