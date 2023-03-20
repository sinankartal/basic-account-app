using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using TransactionService.Models;

namespace Persistence;

public class TransactionRepository : Repository<Transaction>, ITransactionRepository
{
    private readonly TransactionDbContext _context;

    public TransactionRepository(TransactionDbContext context) : base(context)
    {
        _context = context;
    }
    public List<Transaction> GetByAccountIds(List<Guid> accountIds)
    {
        return _context.Transactions.Where(t => accountIds.Contains(t.AccountId)).ToList();
    }
}