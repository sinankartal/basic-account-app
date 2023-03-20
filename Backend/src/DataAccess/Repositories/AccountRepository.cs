using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.IRepositories;
using Persistence.Models;

namespace Persistence;

public class AccountRepository : Repository<Account>, IAccountRepository
{
    private readonly AccountDbContext _context;

    public AccountRepository(AccountDbContext context) : base(context)
    {
        _context = context;
    }


    public Task<List<Account>> GetUserAccounts(Guid userId)
    {
        return _context.Accounts.Where(a => a.UserId.Equals(userId)).ToListAsync();
    }
}