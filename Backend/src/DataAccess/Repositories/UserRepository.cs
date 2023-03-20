using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.IRepositories;
using Persistence.Models;

namespace Persistence;

public class UserRepository : Repository<User>, IUserRepository
{
    private readonly AccountDbContext _context;

    public UserRepository(AccountDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<List<User>> GetAll()
    {
        return _context.Users.ToListAsync();
    }
}