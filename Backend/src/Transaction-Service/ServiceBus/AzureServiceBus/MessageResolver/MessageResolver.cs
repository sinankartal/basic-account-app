using TransactionService.MessageQueue.AzureServiceBus.TransactionResolver;

namespace TransactionService;
public class MessageResolver : IMessageResolver
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IServiceBus _serviceBus;

    public MessageResolver(IServiceBus serviceBus, ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
        _serviceBus = serviceBus;
    }

    public IService Resolve(string message)
    {
        if (message == "AddInitialAmount")
        {
            return new AddInitialAmountService(_transactionRepository);
        }
        else if (message.Equals("GetTransactions"))
        {
            return new GetTransactionsService(_serviceBus, _transactionRepository);
        }
        else
        {
            throw new ArgumentException("Invalid message type");
        }
    }
}