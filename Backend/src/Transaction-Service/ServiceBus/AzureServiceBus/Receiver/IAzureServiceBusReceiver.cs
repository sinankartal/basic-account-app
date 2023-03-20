using Microsoft.Extensions.Configuration;

namespace TransactionService;

public interface IAzureServiceBusReceiver
{
    Task RecieveMessage(IConfiguration config);
}