namespace Infrastructure.Azure.AzureServiceBus.Sender;

public interface IServiceBusSender
{
    Task SendMessageAsync(String message, object data, string correlationId);
}