using System;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.Azure.EventHubs;
using Seq.Input.Azure.EventHub.TestConsole;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Formatting.Compact;

// I want to see internal serilog errors, if Sinks don't work as expected
SelfLog.Enable(Console.Error);

// Load configuration from config
var appSettings = new AppSettings();

// Prepare Event Hub connection string and create Client instance
var connectionStringBuilder = new EventHubsConnectionStringBuilder(appSettings.EventHubConnectionString)
{
    EntityPath = appSettings.EventHubName
};

await using var eventHubClient = new EventHubProducerClient(connectionStringBuilder.ToString());

// Setup local logger with two sinks
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Is(LevelAlias.Minimum)
    .WriteTo.Console() // Writes local log to console
    .WriteTo.AzureEventHub(new CompactJsonFormatter(), eventHubClient) // Writes local log to event hub for demo entries
    .CreateLogger();

Log.Information("Write some test data to event hub");

// Send test data with different levels
Log.Verbose("Test Event");
Log.Debug("Test Event");
Log.Information("Test Event");
Log.Warning("Test Event");
Log.Error("Test Event");
Log.Fatal("Test Event");

Log.Information("Read this data");

await Log.CloseAndFlushAsync();
