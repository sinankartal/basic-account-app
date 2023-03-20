using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using TransactionService.MessageQueue.AzureServiceBus.TransactionResolver;

namespace TransactionService;

public class AzureServiceBusReceiver : IAzureServiceBusReceiver
{
    private readonly IMessageResolver _messageResolver;

    public AzureServiceBusReceiver(IMessageResolver messageResolver)
    {
        _messageResolver = messageResolver;
    }

    public async Task RecieveMessage(IConfiguration config)
    {
        ServiceBusClient client;

        ServiceBusProcessor processor;
        var clientOptions = new ServiceBusClientOptions()
        {
            TransportType = ServiceBusTransportType.AmqpWebSockets
        };

        string connectionString = config["AzureServiceBus:NamespaceConnectionString"];
        string queue = config["AzureServiceBus:Queue"];

        client = new ServiceBusClient(
            connectionString,
            clientOptions);

        processor = client.CreateProcessor(queue, new ServiceBusProcessorOptions());

        try
        {
            processor.ProcessMessageAsync += MessageHandler;

            processor.ProcessErrorAsync += ErrorHandler;

            await processor.StartProcessingAsync();
            Console.ReadKey();

            Console.WriteLine("\nStopping the receiver...");
            await processor.StopProcessingAsync();
            Console.WriteLine("Stopped receiving messages");
        }
        finally
        {
            await processor.DisposeAsync();
            await client.DisposeAsync();
        }

        async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            Console.WriteLine($"Received: {body}");
            var dataProperty = args.Message.ApplicationProperties["data"];

            IService transaction = _messageResolver.Resolve(body);
            string? correlationId = args.Message.ApplicationProperties.ContainsKey("CorrelationId")
                ? args.Message.ApplicationProperties["CorrelationId"].ToString()
                : null;
            transaction.Execute(dataProperty.ToString(), correlationId);
            await args.CompleteMessageAsync(args.Message);
        }

        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}