using TransactionService.Models;
using Newtonsoft.Json;

namespace TransactionService;

public class GetTransactionsService : IService
{
    private readonly ITransactionRepository _transactionRepository;
    private IServiceBus _serviceBus;

    public GetTransactionsService(IServiceBus serviceBus, ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
        _serviceBus = serviceBus;
    }

    public void Execute(string data, string correlationId)
    {
        List<Guid> accountIds = JsonConvert.DeserializeObject<List<Guid>>(data);
        
        List<Transaction> transactions = _transactionRepository.GetByAccountIds(accountIds);

        _serviceBus.SendMessageAsync("TransactionsEmit", transactions, correlationId);
    }
}