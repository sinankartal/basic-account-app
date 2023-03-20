namespace TransactionService;

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
}