using Microsoft.Azure.EventHubs;
using Serilog;
using Serilog.Formatting.Compact;
using System;

namespace Seq.Input.AzureEventHub.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // I want to see internal serilog errors, if Sinks don't work as expected
            Serilog.Debugging.SelfLog.Enable(Console.Error);

            // Load configuration from config
            var appSettings = new AppSettings();

            // Prepare Event Hub connection string and create Client instance
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(appSettings.EventHubConnectionString)
            {
                EntityPath = appSettings.EventHubName
            };
            var eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

            // Setup local logger with two sinks
            Log.Logger = new LoggerConfiguration()
                .WriteTo.ColoredConsole() // Writes local log to console
                .WriteTo.AzureEventHub(new CompactJsonFormatter(), eventHubClient) // Writes local log to event hub for demo entries
                .MinimumLevel.Verbose()
                .CreateLogger();

            Log.Logger.Information("Write some test data to event hub");

            // Send test data with different levels
            Log.Logger.Verbose("Test Event");
            Log.Logger.Debug("Test Event");
            Log.Logger.Information("Test Event");
            Log.Logger.Warning("Test Event");
            Log.Logger.Error("Test Event");
            Log.Logger.Fatal("Test Event");

            Log.Logger.Information("Read this data");

            // Setup listener (only if not running on Seq already)
            var listener = new AzureEventHubListener(new SynchronizedInputWriter(Console.Out), Log.Logger, false,
                appSettings.EventHubConnectionString,
                appSettings.EventHubName,
                appSettings.ConsumerGroupName,
                appSettings.StorageConnectionString,
                appSettings.StorageContainerName);

            Log.Logger.Information("Press any key to shutting down");
            Console.ReadLine();
            Log.Logger.Warning("Please wait - shutting down...");

            // Clean up
            listener.Stop(); // This requires 60 seconds!
            Log.CloseAndFlush();
            eventHubClient.Close();
        }
    }
}