using TransactionService.Models;


public interface ITransactionRepository : IRepository<Transaction>
{
    List<Transaction> GetByAccountIds(List<Guid> accountId);
}