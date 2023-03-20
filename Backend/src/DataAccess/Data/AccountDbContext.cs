using Microsoft.EntityFrameworkCore;
using Persistence.Models;

namespace Persistence.Data
{
    public class AccountDbContext : DbContext
    {
        public AccountDbContext()
        {
        }

        public AccountDbContext(DbContextOptions<AccountDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountMap());
        }

        public DbSet<Account> Accounts { get ; set; }
        public DbSet<User> Users { get; set; }
    }
}