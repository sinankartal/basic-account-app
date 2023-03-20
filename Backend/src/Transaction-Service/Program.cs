using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Persistence.Data;
using Serilog;
using TransactionService.MessageQueue.AzureServiceBus.TransactionResolver;

namespace TransactionService
{
    internal class Program
    {
        private static IConfiguration configuration;

        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            
            var logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File("../logs/console-log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var serviceProvider = new ServiceCollection()
                .AddDbContext<TransactionDbContext>(options =>
                    options.UseInMemoryDatabase(databaseName: "transaction-service"))
                .AddScoped<IMessageResolver, MessageResolver>()
                .AddLogging(builder => builder.AddSerilog(logger)).
                AddSingleton(typeof(ILogger), Log.Logger)
                .AddScoped<ITransactionRepository, TransactionRepository>()
                .AddScoped<IAzureServiceBusReceiver, AzureServiceBusReceiver>()
                .AddScoped<IServiceBusResolver, ServiceBusResolver>()
                .AddScoped<IServiceBus, ServiceBus>()
                .AddSingleton(configuration)
                .BuildServiceProvider();

            var receiver = serviceProvider.GetService<IAzureServiceBusReceiver>();
            receiver.RecieveMessage(configuration);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}