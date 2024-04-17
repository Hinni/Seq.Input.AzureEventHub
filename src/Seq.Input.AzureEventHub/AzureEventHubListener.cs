using System;
using Microsoft.Azure.EventHubs.Processor;
using Serilog;

namespace Seq.Input.AzureEventHub;

sealed class AzureEventHubListener
{
    readonly EventProcessorHost _eventProcessorHost;

    public AzureEventHubListener(SynchronizedInputWriter synchronizedInputWriter, ILogger logger,
        string eventHubConnectionString, string eventHubName, string consumerGroupName,
        string storageConnectionString, string storageContainerName)
    {
        _eventProcessorHost = new EventProcessorHost(
            eventHubName,
            consumerGroupName,
            eventHubConnectionString,
            storageConnectionString,
            storageContainerName)
        {
            PartitionManagerOptions = new PartitionManagerOptions
            {
                RenewInterval = TimeSpan.FromSeconds(10),
                LeaseDuration = TimeSpan.FromSeconds(20),
            }
        };

        var eventProcessorOptions = new EventProcessorOptions
        {
            InvokeProcessorAfterReceiveTimeout = false,
            EnableReceiverRuntimeMetric = false,
            MaxBatchSize = 100,
            ReceiveTimeout = TimeSpan.FromSeconds(25),
        };

        // Registers ClefEventProcessor in running EventProcessorHost instance
        var factory = new InputEventProcessorFactory<ClefEventProcessor>(synchronizedInputWriter, logger);
        _eventProcessorHost.RegisterEventProcessorFactoryAsync(factory, eventProcessorOptions);
    }

    public void Stop()
    {
        // Unregister all registered EventProcessors
        _eventProcessorHost.UnregisterEventProcessorAsync().Wait();
    }
}