namespace TransactionService;

public interface IServiceBusResolver
{
    IServiceBusSender ResolveSender();
}