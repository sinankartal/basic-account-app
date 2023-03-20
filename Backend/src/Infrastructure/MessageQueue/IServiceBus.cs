namespace Infrastructure.Azure;

public interface IServiceBus
{
    Task SendMessageAsync(string message, object data, string correlationId);
    Task<string> ReceiveMessageAsync(string message, string correlationId);
}