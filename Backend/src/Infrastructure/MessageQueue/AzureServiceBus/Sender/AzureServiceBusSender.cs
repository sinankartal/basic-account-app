using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Infrastructure.Azure.AzureServiceBus.Sender;

public class AzureServiceBusSender : IServiceBusSender
{
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;

    public AzureServiceBusSender(IConfiguration configuration, ILogger logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendMessageAsync(string message, object data, string? correlationId)
    {
        _logger.Information($"Send message started for {message}");
        ServiceBusClient client;

        ServiceBusSender sender;

        var clientOptions = new ServiceBusClientOptions()
        {
            TransportType = ServiceBusTransportType.AmqpWebSockets
        };

        string connectionString = _configuration["AzureServiceBus:NamespaceConnectionString"];
        string queue = _configuration["AzureServiceBus:Queue"];

        client = new ServiceBusClient(connectionString, clientOptions);
        sender = client.CreateSender(queue);


        using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

        ServiceBusMessage busMessage = new ServiceBusMessage(message);
        string dataJson = JsonSerializer.Serialize(data);

        busMessage.ApplicationProperties.Add("data", dataJson);
        if (!string.IsNullOrEmpty(correlationId))
        {
            busMessage.ApplicationProperties.Add("CorrelationId", correlationId);
        }

        if (!messageBatch.TryAddMessage(busMessage))
        {
            _logger.Error($"Send message failed for {message} since it is too large.");
            throw new Exception($"The message is too large to fit in the batch.");
        }

        try
        {
            await sender.SendMessagesAsync(messageBatch);
            _logger.Information($"Message: {message} has been published to the queue.");
        }
        finally
        {
            await sender.DisposeAsync();
            await client.DisposeAsync();
            _logger.Information($"Client and sender were disposed.");
        }
    }
}