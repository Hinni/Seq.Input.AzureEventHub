using Microsoft.Azure.EventHubs.Processor;
using Serilog;
using System;

namespace Seq.Input.AzureEventHub
{
    public class AzureEventHubListener
    {
        private readonly EventProcessorHost _eventProcessorHost;

        public AzureEventHubListener(SynchronizedInputWriter synchronizedInputWriter, ILogger logger, bool verboseEnabled,
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
                PartitionManagerOptions = new PartitionManagerOptions()
                {
                    RenewInterval = TimeSpan.FromSeconds(10),
                    LeaseDuration = TimeSpan.FromSeconds(20),
                }
            };

            // Registers ClefEventProcessor in running EventProcessorHost instance
            var factory = new InputEventProcessorFactory<ClefEventProcessor>(synchronizedInputWriter, logger, verboseEnabled);
            _eventProcessorHost.RegisterEventProcessorFactoryAsync(factory, new EventProcessorOptions() { ReceiveTimeout = TimeSpan.FromSeconds(25) });
        }

        public void Stop()
        {
            // Unregister all registered EventProcessors
            _eventProcessorHost.UnregisterEventProcessorAsync().Wait();
        }
    }
}