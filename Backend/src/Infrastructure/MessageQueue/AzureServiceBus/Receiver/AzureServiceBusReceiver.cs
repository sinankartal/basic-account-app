using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Infrastructure.Azure.AzureServiceBus.Sender;

namespace Infrastructure.Azure.AzureServiceBus.Receiver;

public class AzureServiceBusReceiver : IServiceBusReceiver
{
    private readonly IConfiguration _configuration;

    public AzureServiceBusReceiver(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> ReceiveMessageAsync(string message, string correlationId)
    {
        ServiceBusClient client;

        ServiceBusProcessor processor;
        var clientOptions = new ServiceBusClientOptions()
        {
            TransportType = ServiceBusTransportType.AmqpWebSockets
        };

        string connectionString = _configuration["AzureServiceBus:NamespaceConnectionString"];
        string queue = _configuration["AzureServiceBus:Queue"];

        client = new ServiceBusClient(
            connectionString,
            clientOptions);

        processor = client.CreateProcessor(queue, new ServiceBusProcessorOptions());

        try
        {
            var completionSource = new TaskCompletionSource<string>();

            processor.ProcessMessageAsync += async args =>
            {
                var receivedMessage = args.Message;

                if (string.IsNullOrEmpty(correlationId) ||
                    (receivedMessage.ApplicationProperties.ContainsKey("CorrelationId") &&
                     receivedMessage.ApplicationProperties["CorrelationId"].ToString().Equals(correlationId)) &&
                    (!string.IsNullOrEmpty(message) && message == args.Message.Body.ToString()))
                {
                    var dataProperty = receivedMessage.ApplicationProperties["data"];

                    await args.CompleteMessageAsync(receivedMessage);

                    completionSource.SetResult(dataProperty.ToString());
                }
                else
                {
                    await args.AbandonMessageAsync(receivedMessage);
                }
            };

            processor.ProcessErrorAsync += args =>
            {
                Console.WriteLine(args.Exception.ToString());
                return Task.CompletedTask;
            };

            await processor.StartProcessingAsync();

            var result = await completionSource.Task;

            await processor.StopProcessingAsync();

            return result;
        }
        finally
        {
            await processor.DisposeAsync();
            await client.DisposeAsync();
        }
    }
}