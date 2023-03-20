using Microsoft.EntityFrameworkCore;
using TransactionService.Models;

namespace Persistence.Data
{
    public class TransactionDbContext : DbContext
    {
        public TransactionDbContext()
        {
        }

        public TransactionDbContext(DbContextOptions<TransactionDbContext> options)
            : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }
    }
}