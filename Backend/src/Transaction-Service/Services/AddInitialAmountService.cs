using TransactionService.Models;
using Newtonsoft.Json;

namespace TransactionService;

public class AddInitialAmountService : IService
{
    private readonly ITransactionRepository _transactionRepository;

    public AddInitialAmountService(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public void Execute(string data, string correlationId)
    {
        AddInitialAmountMessage transactionData = JsonConvert.DeserializeObject<AddInitialAmountMessage>(data);

        Transaction transaction = new();
        transaction.Amount = transactionData.Amount;
        transaction.AccountId = transactionData.AccountId;

        _transactionRepository.AddAsync(transaction);
    }
}