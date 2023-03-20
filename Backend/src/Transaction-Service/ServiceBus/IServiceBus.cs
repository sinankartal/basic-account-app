namespace TransactionService;

public interface IServiceBus
{
    Task SendMessageAsync(string message, object data, string correlationId);
}