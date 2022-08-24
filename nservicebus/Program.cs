//// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using Sales.Messages;
using NServiceBus;
using NServiceBus.Logging;
using System.Data.SqlClient;

class Program
{
    static ILog log = LogManager.GetLogger<Program>();

    static async Task RunLoop(IEndpointInstance endpointInstance)
    {
        while (true)
        {
            log.Info("Press 'P' to place an order, or 'Q' to quit.");
            var key = Console.ReadKey();
            Console.WriteLine();

            switch (key.Key)
            {
                case ConsoleKey.P:
                    // Instantiate the command
                    var command = new PlaceOrder
                    {
                        OrderId = Guid.NewGuid().ToString()
                    };

                    // Send the command to the local endpoint
                    log.Info($"Sending PlaceOrder command, OrderId = {command.OrderId}");
                    await endpointInstance.Send(command)
                        .ConfigureAwait(false);

                    break;

                case ConsoleKey.Q:
                    return;

                default:
                    log.Info("Unknown input. Please try again.");
                    break;
            }
        }
    }
    static async Task Main()
    {
        Console.Title = "ClientUI";

        var endpointConfiguration = new EndpointConfiguration("ClientUI");
        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Immediate(
            immediate =>
            {
                immediate.NumberOfRetries(1);
            });
        
        recoverability.Delayed(
            delayed =>
            {
                delayed.NumberOfRetries(0);
                delayed.TimeIncrease(TimeSpan.FromMinutes(2));
            });
        var connection = @"Data Source='DESKTOP-2J7HUUE';Initial Catalog=NServiceBus;Integrated Security=True";
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var subscriptions = persistence.SubscriptionSettings();
        subscriptions.CacheFor(TimeSpan.FromMinutes(1));
        persistence.SqlDialect<SqlDialect.MsSqlServer>();
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new SqlConnection(connection);
            });

        var transport = endpointConfiguration.UseTransport<LearningTransport>();
        var routing = transport.Routing();
        routing.RouteToEndpoint(typeof(PlaceOrder), "Sales");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
        .ConfigureAwait(false);
        var defaultFactory = LogManager.Use<DefaultFactory>();
        defaultFactory.Level(LogLevel.Info);

        await RunLoop(endpointInstance)
            .ConfigureAwait(false);

        await endpointInstance.Stop()
            .ConfigureAwait(false);


    }
}
