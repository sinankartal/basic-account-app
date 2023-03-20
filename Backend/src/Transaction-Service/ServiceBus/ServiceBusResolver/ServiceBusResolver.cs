using Microsoft.Extensions.Configuration;
using Serilog;

namespace TransactionService;

public class ServiceBusResolver : IServiceBusResolver
{
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;
    public ServiceBusResolver(IConfiguration configuration, ILogger logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public IServiceBusSender ResolveSender()
    {
        string serviceBusProvicer = _configuration["ServiceBusProvider"];
        
        if (serviceBusProvicer == "Azure")
        {
            return new AzureServiceBusSender(_configuration, _logger);
        }
        else
        {
            throw new ArgumentException("Invalid service provider");
        }
    }
}