
using NServiceBus;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
class Program
{
    static async Task Main()
    {
        Console.Title = "Billing";

        var endpointConfiguration = new EndpointConfiguration("Billing");
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

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press Enter to exit.");
        Console.ReadLine();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}