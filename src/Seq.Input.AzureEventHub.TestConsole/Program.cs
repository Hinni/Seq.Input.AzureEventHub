using Microsoft.Azure.EventHubs;
using Serilog;
using System;
using System.Text;

namespace Seq.Input.AzureEventHub.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Setup local logger
            var log = new LoggerConfiguration()
                .WriteTo.ColoredConsole()
                .MinimumLevel.Verbose()
                .CreateLogger();

            // Load configuration
            var appSettings = new AppSettings();

            // Prepare Event Hub connection string
            var eventHubConnectionString = appSettings.EventHubConnectionString;
            var eventHubName = appSettings.EventHubName;
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(eventHubConnectionString)
            {
                EntityPath = eventHubName
            };

            Console.WriteLine("Write some test data to event hub");

            // Send data
            var eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());
            eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes("{\"@t\":\"2019-02-09T03:44:58.8532799Z\",\"@mt\":\"Hello, {User}\",\"User\":\"nblumhardt\"}")));
            eventHubClient.Close();

            Console.WriteLine("Read this data");

            // Setup listener
            var listener = new AzureEventHubListener(Console.Out, log, true,
                eventHubConnectionString,
                eventHubName,
                appSettings.ConsumerGroupName,
                appSettings.StorageConnectionString,
                appSettings.StorageContainerName);

            Console.WriteLine("Press any key to shutting down");
            Console.ReadLine();
            Console.WriteLine("Please wait - shutting down...");

            // Clean up
            listener.Stop();
        }
    }
}