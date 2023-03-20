namespace TransactionService.MessageQueue.AzureServiceBus.TransactionResolver;

public interface IMessageResolver
{
    IService Resolve(string message);
}