using Microsoft.Extensions.Configuration;

namespace Infrastructure.Azure.AzureServiceBus.Sender;

public interface IServiceBusReceiver
{
    Task<string> ReceiveMessageAsync(string message, string correlationId);
}