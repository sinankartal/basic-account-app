using Infrastructure.Azure.AzureServiceBus.Sender;

namespace TransactionService.MessageQueue.AzureServiceBus.TransactionResolver;

public interface IServiceBusResolver
{
    IServiceBusSender ResolveSender();
    
    IServiceBusReceiver ResolveReceiver();
}