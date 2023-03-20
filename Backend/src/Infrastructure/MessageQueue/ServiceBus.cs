using Infrastructure.Azure.AzureServiceBus.Sender;
using TransactionService.MessageQueue.AzureServiceBus.TransactionResolver;

namespace Infrastructure.Azure;

public class ServiceBus : IServiceBus
{
    private readonly IServiceBusResolver _resolver;

    public ServiceBus(IServiceBusResolver resolver)
    {
        _resolver = resolver;
    }

    public Task SendMessageAsync(string message, object data, string correlationId)
    {
        IServiceBusSender sender = _resolver.ResolveSender();
        return sender.SendMessageAsync(message, data, correlationId);
    }

    public Task<string> ReceiveMessageAsync(string message, string correlationId)
    {
        IServiceBusReceiver receiver = _resolver.ResolveReceiver();
        return receiver.ReceiveMessageAsync(message, correlationId);
    }
}